import { ComponentValue } from "@dojoengine/recs";

export class Character {
  public player_id: string;
  public team_id: number;
  public id: number;
  public role: number;
  public item: number;
  public xp: number;
  public level: number;
  public health: number;
  public attack: number;
  public absorb: number;
  public stun: number;

  constructor(character: ComponentValue) {
    this.player_id = character.player_id.toString(16);
    this.team_id = character.team_id;
    this.id = character.id;
    this.role = character.role;
    this.item = character.item;
    this.xp = character.xp;
    this.level = character.level;
    this.health = character.health;
    this.attack = character.attack;
    this.absorb = character.absorb;
    this.stun = character.stun;
  }
}
