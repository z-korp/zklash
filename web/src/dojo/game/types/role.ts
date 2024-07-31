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
