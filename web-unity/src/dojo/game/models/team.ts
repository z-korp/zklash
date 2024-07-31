import { ComponentValue } from "@dojoengine/recs";

export class Team {
  public player_id: string;
  public id: number;
  public registry_id: number;
  public gold: number;
  public health: number;
  public level: number;
  public character_uuid: number;
  public battle_id: number;
  public foe_squad_id: number;

  constructor(team: ComponentValue) {
    this.player_id = team.player_id.toString(16);
    this.id = team.id;
    this.registry_id = team.registry_id;
    this.gold = team.gold;
    this.health = team.health;
    this.level = team.level;
    this.character_uuid = team.character_uuid;
    this.battle_id = team.battle_id;
    this.foe_squad_id = team.foe_squad_id;
  }
}
