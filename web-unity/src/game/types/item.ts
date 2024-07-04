export enum ItemType {
  None = "None",
  Mushroom = "Mushroom",
  Rock = "Rock",
  Bush = "Bush",
  Pumpkin = "Pumpkin",
}

export enum Size {
  Small = "Small",
  Medium = "Medium",
  Large = "Large",
}

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

export class Item {
  value: ItemType;

  constructor(value: ItemType) {
    this.value = value;
  }

  public isNone(): boolean {
    return this.value === ItemType.None;
  }

  public getImage(size: Size): string {
    switch (this.value) {
      case ItemType.Bush:
        if (size === Size.Small) return bushS;
        else if (size === Size.Medium) return bushM;
        else return bushL;
      case ItemType.Mushroom:
        if (size === Size.Small) return shroomS;
        else if (size === Size.Medium) return shroomM;
        else return shroomL;
      case ItemType.Pumpkin:
        if (size === Size.Small) return pumpkinS;
        else return pumpkinM;
      case ItemType.Rock:
        if (size === Size.Small) return rockS;
        else if (size === Size.Medium) return rockM;
        else return rockL;
      default:
        return "";
    }
  }

  public getTalent(size: Size): string {
    switch (this.value) {
      case ItemType.Bush:
        if (size === Size.Small) return "Absorb 1 damage.";
        else if (size === Size.Medium) return "Absorb 2 damage.";
        else return "Absorb 3 damage.";
      case ItemType.Mushroom:
        if (size === Size.Small) return "Heal 1 health.";
        else if (size === Size.Medium) return "Heal 2 health.";
        else return "Heal 3 health.";
      case ItemType.Pumpkin:
        if (size === Size.Small) return "Save you once.";
        else if (size === Size.Medium) return "Save you twice.";
        else return "";
      case ItemType.Rock:
        if (size === Size.Small) return "Deal 1 damage.";
        else if (size === Size.Medium) return "Deal 2 damage.";
        else return "Deal 3 damage.";
      default:
        return "";
    }
  }
}
