import { ComponentValue } from "@dojoengine/recs";

export class Registry {
  public id: number;
  public squad_count: number;
  public leagues: string;
  public seed: string;

  constructor(registry: ComponentValue) {
    this.id = registry.id;
    this.squad_count = registry.squad_count;
    this.leagues = registry.leagues.toString(16);
    this.seed = registry.seed.toString(16);
  }
}
