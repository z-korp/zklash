import { defineComponent, Type as RecsType, World } from "@dojoengine/recs";

export type ContractComponents = Awaited<
  ReturnType<typeof defineContractComponents>
>;

export function defineContractComponents(world: World) {
  return {
    Character: (() => {
      return defineComponent(
        world,
        {
          player_id: RecsType.String,
          team_id: RecsType.Number,
          id: RecsType.Number,
          role: RecsType.Number,
          item: RecsType.Number,
          xp: RecsType.Number,
          level: RecsType.Number,
          health: RecsType.Number,
          attack: RecsType.Number,
          absorb: RecsType.Number,
          stun: RecsType.Number,
        },
        {
          metadata: {
            name: "zklash-Character",
            types: [
              "ContractAddress",
              "u32",
              "u8",
              "u8",
              "u8",
              "u8",
              "u8",
              "u8",
              "u8",
              "u8",
              "u8",
            ],
          },
        }
      );
    })(),
    Foe: (() => {
      return defineComponent(
        world,
        {
          registry_id: RecsType.Number,
          squad_id: RecsType.Number,
          id: RecsType.Number,
          role: RecsType.Number,
          item: RecsType.Number,
          level: RecsType.Number,
          health: RecsType.Number,
          attack: RecsType.Number,
          absorb: RecsType.Number,
          stun: RecsType.Number,
        },
        {
          metadata: {
            name: "zklash-Foe",
            types: [
              "u32",
              "u32",
              "u8",
              "u8",
              "u8",
              "u8",
              "u8",
              "u8",
              "u8",
              "u8",
            ],
          },
        }
      );
    })(),
    League: (() => {
      return defineComponent(
        world,
        {
          registry_id: RecsType.Number,
          id: RecsType.Number,
          size: RecsType.Number,
        },
        {
          metadata: {
            name: "zklash-League",
            types: ["u32", "u8", "u32"],
          },
        }
      );
    })(),
    Player: (() => {
      return defineComponent(
        world,
        {
          id: RecsType.String,
          name: RecsType.BigInt,
          team_count: RecsType.Number,
          win_count: RecsType.Number,
        },
        {
          metadata: {
            name: "zklash-Player",
            types: ["ContractAddress", "felt252", "u32", "u32"],
          },
        }
      );
    })(),
    Registry: (() => {
      return defineComponent(
        world,
        {
          id: RecsType.Number,
          squad_count: RecsType.Number,
          leagues: RecsType.BigInt,
          seed: RecsType.BigInt,
        },
        {
          metadata: {
            name: "zklash-Registry",
            types: ["u32", "u32", "felt252", "felt252"],
          },
        }
      );
    })(),
    Shop: (() => {
      return defineComponent(
        world,
        {
          player_id: RecsType.String,
          team_id: RecsType.Number,
          reroll_cost: RecsType.Number,
          item_count: RecsType.Number,
          items: RecsType.Number,
          role_count: RecsType.Number,
          roles: RecsType.Number,
        },
        {
          metadata: {
            name: "zklash-Shop",
            types: ["ContractAddress", "u32", "u8", "u8", "u32", "u8", "u32"],
          },
        }
      );
    })(),
    Slot: (() => {
      return defineComponent(
        world,
        {
          registry_id: RecsType.Number,
          league_id: RecsType.Number,
          index: RecsType.Number,
          squad_id: RecsType.Number,
        },
        {
          metadata: {
            name: "zklash-Slot",
            types: ["u32", "u8", "u32", "u32"],
          },
        }
      );
    })(),
    Squad: (() => {
      return defineComponent(
        world,
        {
          registry_id: RecsType.Number,
          id: RecsType.Number,
          league_id: RecsType.Number,
          index: RecsType.Number,
          rating: RecsType.Number,
          size: RecsType.Number,
          name: RecsType.String,
        },
        {
          metadata: {
            name: "zklash-Squad",
            types: ["u32", "u32", "u8", "u32", "u32", "u8", "felt252"],
          },
        }
      );
    })(),
    Team: (() => {
      return defineComponent(
        world,
        {
          player_id: RecsType.String,
          id: RecsType.Number,
          registry_id: RecsType.Number,
          gold: RecsType.Number,
          health: RecsType.Number,
          level: RecsType.Number,
          character_uuid: RecsType.Number,
          battle_id: RecsType.Number,
          foe_squad_id: RecsType.Number,
        },
        {
          metadata: {
            name: "zklash-Team",
            types: [
              "ContractAddress",
              "u32",
              "u32",
              "u32",
              "u32",
              "u8",
              "u8",
              "u8",
              "u32",
            ],
          },
        }
      );
    })(),
  };
}
