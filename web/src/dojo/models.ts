import { ContractComponents } from "./generated/contractModels";
import { Character } from "./game/models/character";
import { Foe } from "./game/models/foe";
import { League } from "./game/models/league";
import { Player } from "./game/models/player";
import { Registry } from "./game/models/registry";
import { Shop } from "./game/models/shop";
import { Slot } from "./game/models/slot";
import { Squad } from "./game/models/squad";
import { Team } from "./game/models/team";

export type ClientModels = ReturnType<typeof models>;

export function models({
  contractModels,
}: {
  contractModels: ContractComponents;
}) {
  return {
    models: {
      ...contractModels,
    },
    classes: {
      Character,
      Foe,
      League,
      Player,
      Registry,
      Shop,
      Slot,
      Squad,
      Team,
    },
  };
}
