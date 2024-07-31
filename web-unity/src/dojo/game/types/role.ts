import bowman from "/assets/mobs/bowman.png";
import knight from "/assets/mobs/knight.png";
import pawn from "/assets/mobs/pawn.png";
import torchoblin from "/assets/mobs/torchoblin.png";
import dynamoblin from "/assets/mobs/dynamoblin.png";
import bomboblin from "/assets/mobs/bomboblin.png";

export enum RoleType {
  None = "None",
  Bomboblin = "Bomboblin",
  Bowman = "Bowman",
  Dynamoblin = "Dynamoblin",
  Knight = "Knight",
  Pawn = "Pawn",
  Torchoblin = "Torchoblin",
}

export class Role {
  value: RoleType;

  constructor(value: RoleType) {
    this.value = value;
  }

  public into(): number {
    return Object.values(RoleType).indexOf(this.value);
  }

  public static from(index: number): Role {
    const item = Object.values(RoleType)[index];
    return new Role(item);
  }

  public isNone(): boolean {
    return this.value === RoleType.None;
  }

  public getName(): string {
    switch (this.value) {
      case RoleType.Bomboblin:
        return "Bomboblin";
      case RoleType.Bowman:
        return "Bowman";
      case RoleType.Dynamoblin:
        return "Dynamoblin";
      case RoleType.Knight:
        return "Knight";
      case RoleType.Pawn:
        return "Pawn";
      case RoleType.Torchoblin:
        return "Torchoblin";
      default:
        return "None";
    }
  }

  public getImage(): string {
    switch (this.value) {
      case RoleType.Knight:
        return knight;
      case RoleType.Bowman:
        return bowman;
      case RoleType.Pawn:
        return pawn;
      case RoleType.Torchoblin:
        return torchoblin;
      case RoleType.Dynamoblin:
        return dynamoblin;
      case RoleType.Bomboblin:
        return bomboblin;
      default:
        return "";
    }
  }

  public getHealth(): number {
    switch (this.value) {
      case RoleType.Knight:
        return 3;
      case RoleType.Bowman:
        return 2;
      case RoleType.Pawn:
        return 2;
      case RoleType.Torchoblin:
        return 4;
      case RoleType.Dynamoblin:
        return 2;
      case RoleType.Bomboblin:
        return 1;
      default:
        return 0;
    }
  }

  public getDamage(): number {
    switch (this.value) {
      case RoleType.Knight:
        return 1;
      case RoleType.Bowman:
        return 2;
      case RoleType.Pawn:
        return 1;
      case RoleType.Torchoblin:
        return 1;
      case RoleType.Dynamoblin:
        return 3;
      case RoleType.Bomboblin:
        return 0;
      default:
        return 0;
    }
  }

  public getTalent(lvl: number): string {
    switch (this.value) {
      case RoleType.Knight:
        if (lvl === 1) return "Get +1 health when an item is equipped.";
        if (lvl === 2) return "Get +2 health when an item is equipped.";
        if (lvl === 3) return "Get +3 health when an item is equipped.";
        return "";
      case RoleType.Bowman:
        if (lvl === 1) return "At death, stun the foe for 1 turn.";
        if (lvl === 2) return "At death, stun the foe for 2 turns.";
        if (lvl === 3) return "At death, stun the foe for 3 turns.";
        return "";
      case RoleType.Pawn:
        if (lvl === 1) return "At death, add (+1/+1) to the next friend.";
        if (lvl === 2) return "At death, add (+2/+2) to the next friend.";
        if (lvl === 3) return "At death, add (+3/+3) to the next friend.";
        return "";
      case RoleType.Torchoblin:
        if (lvl === 1) return "At death, add +1 attack to the next friend.";
        if (lvl === 2) return "At death, add +2 attack to the next friend.";
        if (lvl === 3) return "At death, add +3 attack to the next friend.";
        return "";
      case RoleType.Dynamoblin:
        if (lvl === 1) return "At death, deal 1 damage to the current foe.";
        if (lvl === 2) return "At death, deal 2 damage to the current foe.";
        if (lvl === 3) return "At death, deal 3 damage to the current foe.";
        return "";
      case RoleType.Bomboblin:
        if (lvl === 1)
          return "At death, kill opponent and add +1 health to next friend.";
        if (lvl === 2)
          return "At death, kill opponent and add +2 health to next friend.";
        if (lvl === 3)
          return "At death, kill opponent and add +3 health to next friend.";
        return "";
      default:
        return "";
    }
  }

  public getCost(lvl: number): number {
    switch (this.value) {
      case RoleType.Bomboblin:
        if (lvl === 0) return 0;
        if (lvl === 1) return 30;
        if (lvl === 2) return 90;
        if (lvl === 3) return 180;
        return 0;
      case RoleType.Bowman:
        if (lvl === 0) return 0;
        if (lvl === 1) return 30;
        if (lvl === 2) return 90;
        if (lvl === 3) return 180;
        return 0;
      case RoleType.Dynamoblin:
        if (lvl === 0) return 0;
        if (lvl === 1) return 30;
        if (lvl === 2) return 90;
        if (lvl === 3) return 180;
        return 0;
      case RoleType.Knight:
        if (lvl === 0) return 0;
        if (lvl === 1) return 30;
        if (lvl === 2) return 90;
        if (lvl === 3) return 180;
        return 0;
      case RoleType.Pawn:
        if (lvl === 0) return 0;
        if (lvl === 1) return 30;
        if (lvl === 2) return 90;
        if (lvl === 3) return 180;
        return 0;
      case RoleType.Torchoblin:
        if (lvl === 0) return 0;
        if (lvl === 1) return 30;
        if (lvl === 2) return 90;
        if (lvl === 3) return 180;
        return 0;
      default:
        return 0;
    }
  }
}
