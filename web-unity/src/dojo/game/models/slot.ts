import { ComponentValue } from "@dojoengine/recs";

export class Slot {
  public registry_id: number;
  public league_id: number;
  public index: number;
  public squad_id: number;

  constructor(slot: ComponentValue) {
    this.registry_id = slot.registry_id;
    this.league_id = slot.league_id;
    this.index = slot.index;
    this.squad_id = slot.squad_id;
  }
}
