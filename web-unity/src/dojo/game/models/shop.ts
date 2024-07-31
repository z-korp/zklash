import { ComponentValue } from "@dojoengine/recs";
import { Packer } from "../helpers/packer";
import { Role } from "../types/role";
import { Item } from "../types/item";
import { ITEM_BIT_COUNT, ROLE_BIT_COUNT } from "../constants";

export class Shop {
  public player_id: string;
  public team_id: number;
  public purchase_cost: number;
  public reroll_cost: number;
  public item_count: number;
  public items: Item[];
  public role_count: number;
  public roles: Role[];

  constructor(shop: ComponentValue) {
    this.player_id = shop.player_id.toString(16);
    this.team_id = shop.team_id;
    this.purchase_cost = shop.purchase_cost;
    this.reroll_cost = shop.reroll_cost;
    this.item_count = shop.item_count;
    this.items = Packer.sized_unpack(BigInt(shop.items), ITEM_BIT_COUNT, 3).map(
      (item) => Item.from(item),
    );
    this.role_count = shop.role_count;
    this.roles = Packer.sized_unpack(BigInt(shop.roles), ROLE_BIT_COUNT, 3).map(
      (role) => Role.from(role),
    );
  }
}
