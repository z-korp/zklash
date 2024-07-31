import bushS from "/assets/items/bushS.png";
import bushM from "/assets/items/bushM.png";
import bushL from "/assets/items/bushL.png";
import shroomS from "/assets/items/shroomS.png";
import shroomM from "/assets/items/shroomM.png";
import shroomL from "/assets/items/shroomL.png";
import pumpkinS from "/assets/items/pumpkinS.png";
import pumpkinM from "/assets/items/pumpkinM.png";
import rockS from "/assets/items/rockS.png";
import rockM from "/assets/items/rockM.png";
import rockL from "/assets/items/rockL.png";

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
  category: ItemCategory;
  size: ItemSize;

  constructor(value: ItemType) {
    this.value = value;
    if (value.includes("Small")) {
      this.size = ItemSize.Small;
    } else if (value.includes("Medium")) {
      this.size = ItemSize.Medium;
    } else if (value.includes("Large")) {
      this.size = ItemSize.Large;
    } else {
      this.size = ItemSize.None;
    }

    if (value.includes("Mushroom")) {
      this.category = ItemCategory.Mushroom;
    } else if (value.includes("Rock")) {
      this.category = ItemCategory.Rock;
    } else if (value.includes("Bush")) {
      this.category = ItemCategory.Bush;
    } else if (value.includes("Pumpkin")) {
      this.category = ItemCategory.Pumpkin;
    } else {
      this.category = ItemCategory.None;
    }
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
    return this.category;
  }

  public getSize(): ItemSize {
    return this.size;
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

  public getImage(size: ItemSize): string {
    switch (this.category) {
      case ItemCategory.Bush:
        if (size === ItemSize.Small) return bushS;
        else if (size === ItemSize.Medium) return bushM;
        else return bushL;
      case ItemCategory.Mushroom:
        if (size === ItemSize.Small) return shroomS;
        else if (size === ItemSize.Medium) return shroomM;
        else return shroomL;
      case ItemCategory.Pumpkin:
        if (size === ItemSize.Small) return pumpkinS;
        else return pumpkinM;
      case ItemCategory.Rock:
        if (size === ItemSize.Small) return rockS;
        else if (size === ItemSize.Medium) return rockM;
        else return rockL;
      default:
        return "";
    }
  }

  public getTalent(size: ItemSize): string {
    switch (this.category) {
      case ItemCategory.Bush:
        if (size === ItemSize.Small) return "Absorb 1 damage.";
        else if (size === ItemSize.Medium) return "Absorb 2 damage.";
        else return "Absorb 3 damage.";
      case ItemCategory.Mushroom:
        if (size === ItemSize.Small) return "Give +1 health.";
        else if (size === ItemSize.Medium) return "Give +2 health.";
        else return "Give +3 health.";
      case ItemCategory.Pumpkin:
        if (size === ItemSize.Small) return "Save you once.";
        else if (size === ItemSize.Medium) return "Save you twice.";
        else return "";
      case ItemCategory.Rock:
        if (size === ItemSize.Small) return "Deals 1 damage once to the foe";
        else if (size === ItemSize.Medium)
          return "Deals 2 damages once to the foe";
        else return "Deals 3 damages once to the foe";
      default:
        return "";
    }
  }

  public getCost(size: ItemSize): number {
    switch (this.category) {
      case ItemCategory.Bush:
        if (size === ItemSize.Small) return 30;
        if (size === ItemSize.Medium) return 60;
        if (size === ItemSize.Large) return 90;
        return 0;
      case ItemCategory.Mushroom:
        if (size === ItemSize.Small) return 30;
        if (size === ItemSize.Medium) return 60;
        if (size === ItemSize.Large) return 90;
        return 0;
      case ItemCategory.Pumpkin:
        if (size === ItemSize.Small) return 30;
        if (size === ItemSize.Medium) return 60;
        if (size === ItemSize.Large) return 90;
        return 0;
      case ItemCategory.Rock:
        if (size === ItemSize.Small) return 30;
        if (size === ItemSize.Medium) return 60;
        if (size === ItemSize.Large) return 90;
        return 0;
      default:
        return 0;
    }
  }
}
