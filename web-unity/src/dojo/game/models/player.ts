import { ComponentValue } from "@dojoengine/recs";
import { shortString } from "starknet";

export class Player {
  public id: string;
  public name: string;
  public team_count: number;
  public win_count: number;

  constructor(player: ComponentValue) {
    this.id = player.id.toString(16);
    this.name = shortString.decodeShortString(player.name);
    this.team_count = player.team_count;
    this.win_count = player.win_count;
  }
}
