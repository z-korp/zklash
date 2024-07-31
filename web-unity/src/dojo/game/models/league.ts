import { ComponentValue } from "@dojoengine/recs";

export class League {
  public registry_id: number;
  public id: number;
  public size: number;

  constructor(league: ComponentValue) {
    this.registry_id = league.registry_id;
    this.id = league.id;
    this.size = league.size;
  }
}
