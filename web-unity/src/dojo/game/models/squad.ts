import { ComponentValue } from "@dojoengine/recs";
import { shortString } from "starknet";

export class Squad {
  public registry_id: number;
  public id: number;
  public league_id: number;
  public index: number;
  public rating: number;
  public size: number;
  public name: string;

  constructor(squad: ComponentValue) {
    this.registry_id = squad.registry_id;
    this.id = squad.id;
    this.league_id = squad.league_id;
    this.index = squad.index;
    this.rating = squad.rating;
    this.size = squad.size;
    this.name = shortString.decodeShortString(squad.name);
  }
}
