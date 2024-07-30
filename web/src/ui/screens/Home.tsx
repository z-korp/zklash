import { useComponentValue, useEntityQuery } from "@dojoengine/react";
import { Entity, Has, HasValue, getComponentValue } from "@dojoengine/recs";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { getEntityIdFromKeys } from "@dojoengine/utils";
import { useDojo } from "../../dojo/useDojo";
import { Button } from "../elements/button";
import { Input } from "@/ui/elements/input";
import Registry from "@/ui/components/Registry";
import { Separator } from "../elements/separator";
import { Badge } from "../elements/badge";

function Home() {
  const {
    setup: {
      systemCalls: {
        create,
        spawn,
        hire,
        equip,
        reroll,
        start,
        hydrate,
        sell,
        merge,
        xp,
      },
      clientModels: {
        models: {
          Player: PlayerModel,
          Team: TeamModel,
          Shop: ShopModel,
          Character: CharacterModel,
          Registry: RegistryModel,
        },
        classes: { Player, Team, Shop, Character },
      },
    },
    account: { account },
  } = useDojo();

  const dragItem = useRef<typeof CharacterModel | null>(null);
  const dragOverItem = useRef<typeof CharacterModel | null>(null);
  const [name, setName] = useState("");
  const [characters, setCharacters] = useState<(typeof Character)[]>([]);

  // Player
  const playerKey = useMemo(() => {
    return getEntityIdFromKeys([BigInt(account.address)]) as Entity;
  }, [account]);
  const playerComponent = useComponentValue(PlayerModel, playerKey);
  const player = useMemo(() => {
    if (!playerComponent) return null;
    return new Player(playerComponent);
  }, [playerComponent]);

  // Team
  const teamKey = useMemo(() => {
    return getEntityIdFromKeys([
      BigInt(account.address),
      BigInt(player?.team_count || 0),
    ]) as Entity;
  }, [account, player]);
  const teamComponent = useComponentValue(TeamModel, teamKey);
  const team = useMemo(() => {
    if (!teamComponent) return null;
    return new Team(teamComponent);
  }, [teamComponent]);

  // Shop
  const shopKey = useMemo(() => {
    return getEntityIdFromKeys([
      BigInt(account.address),
      BigInt(player?.team_count || 0),
    ]) as Entity;
  }, [account, player]);
  const shopComponent = useComponentValue(ShopModel, shopKey);
  const shop = useMemo(() => {
    if (!shopComponent) return null;
    return new Shop(shopComponent);
  }, [shopComponent]);

  // Characters
  const characterKeys = useEntityQuery([
    Has(CharacterModel),
    HasValue(CharacterModel, { player_id: player?.id, team_id: team?.id || 0 }),
  ]);
  useEffect(() => {
    if (!characterKeys) return;
    const newCharacters: any[] = [];
    characterKeys.forEach((key) => {
      const characterComponent = getComponentValue(CharacterModel, key);
      if (!characterComponent || !characterComponent.role) return;
      newCharacters.push(new Character(characterComponent));
    });
    setCharacters(newCharacters);
  }, [characterKeys]);

  // Registry
  const registryKey = useMemo(() => {
    return getEntityIdFromKeys([BigInt(team?.registry_id || 1)]) as Entity;
  }, [team]);
  const registry = useComponentValue(RegistryModel, registryKey);

  const handleName = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setName(e.target.value);
  }, []);

  const order = useMemo(() => {
    if (!characters) return "0x0";
    const ids = characters.map((character) => character.id);
    // Pack indexes
    let packed = 0;
    ids.reverse().forEach((id) => {
      packed <<= 8;
      packed |= id;
    });
    return `0x${packed.toString(16)}`;
  }, [characters]);

  const dragStart = useCallback(
    (e: React.DragEvent<HTMLDivElement>) => {
      dragItem.current = characters.find(
        (character) => character.id == e.currentTarget.id,
      );
    },
    [characters],
  );

  const dragEnter = useCallback(
    (e: React.DragEvent<HTMLDivElement>) => {
      dragOverItem.current = characters.find(
        (character) => character.id == e.currentTarget.id,
      );
    },
    [characters],
  );

  const dropMob = useCallback(
    (index: number) => {
      if (
        !account ||
        !team ||
        dragItem.current === null ||
        dragOverItem.current === null
      )
        return;
      xp({
        account,
        team_id: team.id,
        character_id: parseInt(dragOverItem.current.id),
        index,
      });
      dragOverItem.current = null;
      dragItem.current = null;
    },
    [account, team, characters],
  );

  const dropItem = useCallback(
    (index: number) => {
      if (
        !account ||
        !team ||
        dragItem.current === null ||
        dragOverItem.current === null
      )
        return;
      equip({
        account,
        team_id: team.id,
        character_id: parseInt(dragOverItem.current.id),
        index,
      });
      dragOverItem.current = null;
      dragItem.current = null;
    },
    [account, team, characters],
  );

  const mergeMob = useCallback(() => {
    if (
      !account ||
      !team ||
      dragItem.current === null ||
      dragOverItem.current === null
    )
      return;
    merge({
      account,
      team_id: team.id,
      from_id: parseInt(dragItem.current.id),
      to_id: parseInt(dragOverItem.current.id),
    });
    dragOverItem.current = null;
    dragItem.current = null;
  }, [account, team, characters]);

  const swap = useCallback(
    (a: number, b: number) => {
      const newCharacters = [...characters];
      const temp = newCharacters[a];
      newCharacters[a] = newCharacters[b];
      newCharacters[b] = temp;
      setCharacters(newCharacters);
    },
    [characters],
  );

  return (
    <div className="flex flex-col gap-4 w-full items-center">
      <h1>zKlash</h1>
      <div className="flex gap-4 justify-center items-stretch">
        <div className="flex flex-col items-center gap-2 border rounded p-4">
          <h2>PLAYER</h2>
          <Separator />
          {player && <p>{player.name}</p>}
          {!player && <Input value={name} onChange={handleName} />}
          {!player && (
            <Button onClick={() => create({ account, name })}>
              Create Player
            </Button>
          )}
          {player && <p>{`Team count: ${player.team_count}`}</p>}
          {player && <p>{`Win count: ${player.win_count}`}</p>}
          {player && <Button onClick={() => spawn({ account })}>Spawn</Button>}
        </div>
        <div className="flex flex-col items-center gap-2 border rounded p-4">
          <h2>TEAM</h2>
          <Separator />
          {team && <p>{`Team ID: ${team.id}`}</p>}
          {team && <p>{`Registry ID: ${team.registry_id}`}</p>}
          {team && <p>{`Gold: ${team.gold}`}</p>}
          {team && <p>{`Health: ${team.health}`}</p>}
          {team && <p>{`Level: ${team.level}`}</p>}
          {team && <p>{`Character UUID: ${team.character_uuid}`}</p>}
          {team && <p>{`Battle ID: ${team.battle_id}`}</p>}
          {team && <p>{`Foe Squad ID: ${team.foe_squad_id}`}</p>}
          {team && characters && (
            <Button
              onClick={() =>
                start({ account, team_id: team.id, order: parseInt(order, 16) })
              }
            >
              Fight
            </Button>
          )}
        </div>
        <div className="flex flex-col items-center gap-2 border rounded p-4">
          <h2>SHOP</h2>
          <Separator />
          {shop && <p>{`Purchase Cost: ${shop.purchase_cost}`}</p>}
          {shop && <p>{`Reroll cost: ${shop.reroll_cost}`}</p>}
          {team && shop && (
            <Button onClick={() => reroll({ account, team_id: team.id })}>
              Reroll
            </Button>
          )}
          {shop && <p>{`Item count: ${shop.item_count}`}</p>}
          {team &&
            shop &&
            shop.items &&
            shop.items.map((item, index) => (
              <div
                className="ml-2 flex gap-2"
                key={index}
                id={`${index}`}
                onDragStart={(e) => dragStart(e)}
                onDragEnter={(e) => dragEnter(e)}
                onDragEnd={() => dropItem(index)}
                draggable
              >
                {(item && (
                  <Button
                    onClick={() =>
                      equip({
                        account,
                        team_id: team.id,
                        character_id: 0,
                        index,
                      })
                    }
                  >
                    {item.getName()}
                  </Button>
                )) ||
                  null}
              </div>
            ))}
          {shop && <p>{`Role count: ${shop.role_count}`}</p>}
          {team &&
            shop &&
            shop.roles &&
            shop.roles.map((role, index) => (
              <div
                className="ml-2 flex gap-2"
                key={index}
                id={`${index}`}
                onDragStart={(e) => dragStart(e)}
                onDragEnter={(e) => dragEnter(e)}
                onDragEnd={() => dropMob(index)}
                draggable
              >
                {(role && (
                  <Button
                    onClick={() => hire({ account, team_id: team.id, index })}
                  >
                    {role.getName()}
                  </Button>
                )) ||
                  null}
              </div>
            ))}
        </div>
        <div className="flex flex-col items-stretch gap-2 border rounded p-4">
          <h2>MOBS</h2>
          <Separator />
          {characters &&
            characters.map((character, index) => (
              <div
                className="flex gap-2"
                id={character.id}
                key={index}
                onDragStart={(e) => dragStart(e)}
                onDragEnter={(e) => dragEnter(e)}
                onDragEnd={mergeMob}
                draggable
              >
                <div className="flex flex-col gap-1 text-xs">
                  <div
                    className={`${index === 0 ? "opacity-50" : "cursor-pointer"}`}
                    onClick={() => index !== 0 && swap(index, index - 1)}
                  >
                    ⬆️
                  </div>
                  <div
                    className={`${index + 1 === characters.length ? "opacity-50" : "cursor-pointer"}`}
                    onClick={() =>
                      index + 1 !== characters.length && swap(index, index + 1)
                    }
                  >
                    ⬇️
                  </div>
                </div>
                {(character && (
                  <Badge className={`grow ${index >= 4 ? "opacity-50" : ""}`}>
                    <div className="flex flex-col">
                      <p>{`[${character.id}] ${character.role.getName()} Lv${character.level}`}</p>
                      {(character.item && (
                        <p className="text-xs">{`- ${character.item.getName()}`}</p>
                      )) ||
                        null}
                    </div>
                  </Badge>
                )) ||
                  null}
                {(character && team && (
                  <Button
                    className="h-full"
                    onClick={() =>
                      sell({
                        account,
                        team_id: team.id,
                        character_id: character.id,
                      })
                    }
                  >
                    Sell
                  </Button>
                )) ||
                  null}
              </div>
            ))}
          {characters && <p>{`Order: ${order}`}</p>}
        </div>
      </div>
      <div className="flex flex-col justify-center items-center gap-2 border rounded p-4">
        <h2>REGISTRY</h2>
        <Separator />
        {registry && registry.leagues === BigInt(0) && (
          <Button onClick={() => hydrate({ account })}>Hydrate</Button>
        )}
        {registry && registry.leagues > BigInt(0) && (
          <Registry registry={registry.id} />
        )}
      </div>
    </div>
  );
}

export default Home;
