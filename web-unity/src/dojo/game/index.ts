export enum Item {
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

export const getItemName = (id: number): Item => {
  return Object.values(Item)[id];
};

export const getItemId = (item: Item): number => {
  return Object.keys(Item).indexOf(item);
};

export enum Role {
  None = "None",
  Knight = "Knight",
  Bowman = "Bowman",
  Pawn = "Pawn",
  Torchoblin = "Torchoblin",
  Dynamoblin = "Dynamoblin",
  Bomboblin = "Bomboblin",
}

export const getRoleName = (id: number): Role => {
  return Object.values(Role)[id];
};

export const getRoleId = (role: Role): number => {
  return Object.keys(Role).indexOf(role);
};
