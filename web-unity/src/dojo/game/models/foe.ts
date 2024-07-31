import { ComponentValue } from "@dojoengine/recs";
import { Role } from "../types/role";
import { Item } from "../types/item";

export class Foe {
  public registry_id: number;
  public squad_id: number;
  public id: number;
  public role: Role;
  public item: Item;
  public level: number;
  public health: number;
  public attack: number;
  public absorb: number;
  public stun: number;

  constructor(foe: ComponentValue) {
    this.registry_id = foe.registry_id;
    this.squad_id = foe.squad_id;
    this.id = foe.id;
    this.role = Role.from(foe.role);
    this.item = Item.from(foe.item);
    this.level = foe.level;
    this.health = foe.health;
    this.attack = foe.attack;
    this.absorb = foe.absorb;
    this.stun = foe.stun;
  }
}
