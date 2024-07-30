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
}
