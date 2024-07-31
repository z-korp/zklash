using System;

namespace zKlash.Game.Items
{
    public enum Size
    {
        Small,
        Medium,
        Large
    }

    public enum Item
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
        Item GetItemType();

        int Health(Phase phase);
        int Attack(Phase phase);
        int Damage(Phase phase);
        int Absorb(Phase phase);
        Item Usage(Phase phase);
        int Cost();
    }

    public class NoneItem : IItem
    {
        public Item GetItemType() => Item.None;

        public int Health(Phase phase) => 0;
        public int Attack(Phase phase) => 0;
        public int Damage(Phase phase) => 0;
        public int Absorb(Phase phase) => 0;
        public Item Usage(Phase phase) => Item.None;
        public int Cost() => 0;
    }

    public static class ItemFactory
    {
        public static IItem GetItem(Item item)
        {
            switch (item)
            {
                case Item.None:
                    return new NoneItem();
                case Item.MushroomSmall:
                    return new Mushroom(Size.Small);
                case Item.RockSmall:
                    return new Rock(Size.Small);
                case Item.BushSmall:
                    return new Bush(Size.Small);
                case Item.PumpkinSmall:
                    return new Pumpkin(Size.Small);
                case Item.MushroomMedium:
                    return new Mushroom(Size.Medium);
                case Item.RockMedium:
                    return new Rock(Size.Medium);
                case Item.BushMedium:
                    return new Bush(Size.Medium);
                case Item.PumpkinMedium:
                    return new Pumpkin(Size.Medium);
                case Item.MushroomLarge:
                    return new Mushroom(Size.Large);
                case Item.RockLarge:
                    return new Rock(Size.Large);
                case Item.BushLarge:
                    return new Bush(Size.Large);
                case Item.PumpkinLarge:
                    return new Pumpkin(Size.Large);
                default:
                    throw new ArgumentException("Invalid item type");
            }
        }
    }
}