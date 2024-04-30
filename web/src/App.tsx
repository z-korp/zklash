import { useComponentValue, useEntityQuery } from "@dojoengine/react";
import { Entity, Has, HasValue, getComponentValue } from "@dojoengine/recs";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import "./App.css";
import { getEntityIdFromKeys } from "@dojoengine/utils";
import { useDojo } from "./dojo/useDojo";
import { Button } from "./ui/elements/button";
import { Input } from "@/ui/elements/input";
import Registry from "@/ui/components/Registry";
import { shortString } from "starknet";
import { Separator } from "./ui/elements/separator";
import { getItemName, getRoleName } from "@/dojo/game";
import { Badge } from "./ui/elements/badge";

function App() {
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
      clientComponents: {
        Player: PlayerModel,
        Team: TeamModel,
        Shop: ShopModel,
        Character: CharacterModel,
        Foe: FoeModel,
        Registry: RegistryModel,
        Squad: SquadModel,
        League: LeagueModel,
        Slot: SlotModel,
      },
    },
    account: { account },
  } = useDojo();

  const dragItem = useRef<typeof CharacterModel | null>(null);
  const dragOverItem = useRef<typeof CharacterModel | null>(null);
  const [name, setName] = useState("");
  const [characters, setCharacters] = useState<any[]>([]);

  // Player
  const playerKey = useMemo(() => {
    return getEntityIdFromKeys([BigInt(account.address)]) as Entity;
  }, [account]);
  const player = useComponentValue(PlayerModel, playerKey);

  // Team
  const teamKey = useMemo(() => {
    return getEntityIdFromKeys([
      BigInt(account.address),
      BigInt(player?.team_count || 0),
    ]) as Entity;
  }, [account, player]);
  const team = useComponentValue(TeamModel, teamKey);

  // Shop
  const shopKey = useMemo(() => {
    return getEntityIdFromKeys([
      BigInt(account.address),
      BigInt(player?.team_count || 0),
    ]) as Entity;
  }, [account, player]);
  const shop = useComponentValue(ShopModel, shopKey);

  // Characters
  const characterKeys = useEntityQuery([
    Has(CharacterModel),
    HasValue(CharacterModel, { team_id: team?.id || 0 }),
  ]);
  useEffect(() => {
    if (!characterKeys) return;
    const newCharacters: any[] = [];
    characterKeys.forEach((key) => {
      const character = getComponentValue(CharacterModel, key);
      if (!character || !character.role) return;
      newCharacters.push(character);
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

  const items = useMemo(() => {
    if (!shop) return [];
    let packed = shop.items;
    const items = [];
    while (packed > 0) {
      items.push(packed & 0xff);
      packed >>= 8;
    }
    return items;
  }, [shop]);

  const roles = useMemo(() => {
    if (!shop) return [];
    let packed = shop.roles;
    const roles = [];
    while (packed > 0) {
      roles.push(packed & 0xff);
      packed >>= 8;
    }
    return roles;
  }, [shop]);

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
      console.log("dropMob", index);
      xp(account, team.id, parseInt(dragOverItem.current.id), index);
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
      equip(account, team.id, parseInt(dragOverItem.current.id), index);
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
    merge(
      account,
      team.id,
      parseInt(dragItem.current.id),
      parseInt(dragOverItem.current.id),
    );
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
    <div className="flex flex-col gap-4 w-full items-center mt-20">
      <h1>zKlash</h1>
      <div className="flex gap-4 justify-center items-stretch">
        <div className="flex flex-col items-center gap-2 border rounded p-4">
          <h2>PLAYER</h2>
          <Separator />
          {player && (
            <p>
              {shortString.decodeShortString(`0x${player.name.toString(16)}`)}
            </p>
          )}
          {!player && <Input value={name} onChange={handleName} />}
          {!player && (
            <Button onClick={() => create(account, name)}>Create Player</Button>
          )}
          {player && <p>{`Team count: ${player.team_count}`}</p>}
          {player && <p>{`Win count: ${player.win_count}`}</p>}
          {player && <Button onClick={() => spawn(account)}>Spawn</Button>}
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
          {team && <p>{`Size: ${team.size}`}</p>}
          {team && <p>{`Battle ID: ${team.battle_id}`}</p>}
          {team && <p>{`Foe Squad ID: ${team.foe_squad_id}`}</p>}
          {team && characters && (
            <Button
              onClick={() => start(account, team.id, parseInt(order, 16))}
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
            <Button onClick={() => reroll(account, team.id)}>Reroll</Button>
          )}
          {shop && <p>{`Item count: ${shop.item_count}`}</p>}
          {team &&
            shop &&
            items &&
            items.map((item, index) => (
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
                  <Button onClick={() => equip(account, team.id, 0, index)}>
                    {getItemName(item)}
                  </Button>
                )) ||
                  null}
              </div>
            ))}
          {shop && <p>{`Role count: ${shop.role_count}`}</p>}
          {team &&
            shop &&
            roles &&
            roles.map((role, index) => (
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
                  <Button onClick={() => hire(account, team.id, index)}>
                    {getRoleName(role)}
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
                      <p>{`[${character.id}] ${getRoleName(character.role)} Lv${character.level}`}</p>
                      {(character.item && (
                        <p className="text-xs">{`- ${getItemName(character.item)}`}</p>
                      )) ||
                        null}
                    </div>
                  </Badge>
                )) ||
                  null}
                {(character && team && (
                  <Button
                    className="h-full"
                    onClick={() => sell(account, team.id, character.id)}
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
          <Button onClick={() => hydrate(account)}>Hydrate</Button>
        )}
        {registry && registry.leagues > BigInt(0) && (
          <Registry registry={registry.id} />
        )}
      </div>
    </div>
  );
}

export default App;
