using System;

namespace zKlash.Game.Items
{
    public enum Size
    {
        Small,
        Medium,
        Large
    }

    public enum ItemEnum
    {
        None,
        MushroomSmall,
        MushroomMedium,
        MushroomLarge,
        RockSmall,
        RockMedium,
        RockLarge,
        BushSmall,
        BushMedium,
        BushLarge,
        PumpkinSmall,
        PumpkinMedium,
        PumpkinLarge,
    }

    public interface IItem
    {
        int Health(Phase phase);
        int Attack(Phase phase);
        int Damage(Phase phase);
        int Absorb(Phase phase);
        ItemEnum Usage(Phase phase);
    }

    public class NoneItem : IItem
    {
        public int Health(Phase phase) => 0;
        public int Attack(Phase phase) => 0;
        public int Damage(Phase phase) => 0;
        public int Absorb(Phase phase) => 0;
        public ItemEnum Usage(Phase phase) => ItemEnum.None;
    }

    public static class ItemFactory
    {
        public static IItem GetItem(ItemEnum item)
        {
            switch (item)
            {
                case ItemEnum.None:
                    return new NoneItem();
                case ItemEnum.MushroomSmall:
                    return new Mushroom(Size.Small);
                case ItemEnum.RockSmall:
                    return new Rock(Size.Small);
                case ItemEnum.BushSmall:
                    return new Bush(Size.Small);
                case ItemEnum.PumpkinSmall:
                    return new Pumpkin(Size.Small);
                case ItemEnum.MushroomMedium:
                    return new Mushroom(Size.Medium);
                case ItemEnum.RockMedium:
                    return new Rock(Size.Medium);
                case ItemEnum.BushMedium:
                    return new Bush(Size.Medium);
                case ItemEnum.PumpkinMedium:
                    return new Pumpkin(Size.Medium);
                case ItemEnum.MushroomLarge:
                    return new Mushroom(Size.Large);
                case ItemEnum.RockLarge:
                    return new Rock(Size.Large);
                case ItemEnum.BushLarge:
                    return new Bush(Size.Large);
                case ItemEnum.PumpkinLarge:
                    return new Pumpkin(Size.Large);
                default:
                    throw new ArgumentException("Invalid item type");
            }
        }
    }
}