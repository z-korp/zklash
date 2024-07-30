export enum ItemType {
  None = "None",
  MushroomSmall = "MushroomSmall",
  MushroomMedium = "MushroomMedium",
  MushroomLarge = "MushroomLarge",
  RockSmall = "RockSmall",
  RockMedium = "RockMedium",
  RockLarge = "RockLarge",
  BushSmall = "BushSmall",
  BushMedium = "BushMedium",
  BushLarge = "BushLarge",
  PumpkinSmall = "PumpkinSmall",
  PumpkinMedium = "PumpkinMedium",
  PumpkinLarge = "PumpkinLarge",
}

export enum ItemCategory {
  None = "None",
  Mushroom = "Mushroom",
  Rock = "Rock",
  Bush = "Bush",
  Pumpkin = "Pumpkin",
}

export enum ItemSize {
  None = "None",
  Small = "Small",
  Medium = "Medium",
  Large = "Large",
}

export class Item {
  value: ItemType;

  constructor(value: ItemType) {
    this.value = value;
  }

  public into(): number {
    return Object.values(ItemType).indexOf(this.value);
  }

  public static from(index: number): Item {
    const item = Object.values(ItemType)[index];
    return new Item(item);
  }

  public isNone(): boolean {
    return this.value === ItemType.None;
  }

  public getCategory(): ItemCategory {
    if (this.value.includes("Mushroom")) {
      return ItemCategory.Mushroom;
    }
    if (this.value.includes("Rock")) {
      return ItemCategory.Rock;
    }
    if (this.value.includes("Bush")) {
      return ItemCategory.Bush;
    }
    if (this.value.includes("Pumpkin")) {
      return ItemCategory.Pumpkin;
    }
    return ItemCategory.None;
  }

  public getSize(): ItemSize {
    if (this.value.includes("Small")) {
      return ItemSize.Small;
    }
    if (this.value.includes("Medium")) {
      return ItemSize.Medium;
    }
    if (this.value.includes("Large")) {
      return ItemSize.Large;
    }
    return ItemSize.None;
  }

  public getName(): string {
    switch (this.value) {
      case ItemType.MushroomSmall:
        return "MushroomSmall";
      case ItemType.MushroomMedium:
        return "MushroomMedium";
      case ItemType.MushroomLarge:
        return "MushroomLarge";
      case ItemType.RockSmall:
        return "RockSmall";
      case ItemType.RockMedium:
        return "RockMedium";
      case ItemType.RockLarge:
        return "RockLarge";
      case ItemType.BushSmall:
        return "BushSmall";
      case ItemType.BushMedium:
        return "BushMedium";
      case ItemType.BushLarge:
        return "BushLarge";
      case ItemType.PumpkinSmall:
        return "PumpkinSmall";
      case ItemType.PumpkinMedium:
        return "PumpkinMedium";
      case ItemType.PumpkinLarge:
        return "PumpkinLarge";
      default:
        return "None";
    }
  }
}
