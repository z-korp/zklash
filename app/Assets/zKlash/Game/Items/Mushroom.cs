namespace zKlash.Game.Items
{
    public class Mushroom : IItem
    {
        private Size _size;

        public Mushroom(Size size)
        {
            _size = size;
        }

        public Item GetItemType()
        {
            if (_size == Size.Small)
            {
                return Item.MushroomSmall;
            }
            else if (_size == Size.Medium)
            {
                return Item.MushroomMedium;
            }
            else if (_size == Size.Large)
            {
                return Item.MushroomLarge;
            }
            else
            {
                return Item.None;
            }
        }

        public int Health(Phase phase)
        {
            if (phase == Phase.OnEquip)
            {
                return _size switch
                {
                    Size.Small => 1,
                    Size.Medium => 2,
                    Size.Large => 3,
                    _ => 0
                };
            }
            return 0;
        }

        public int Attack(Phase phase)
        {
            return 0;
        }

        public int Damage(Phase phase)
        {
            return 0;
        }

        public int Absorb(Phase phase)
        {
            return 0;
        }

        public Item Usage(Phase phase)
        {
            switch (_size)
            {
                case Size.Small: return Item.MushroomSmall;
                case Size.Medium: return Item.MushroomMedium;
                case Size.Large: return Item.MushroomLarge;
                default: return Item.None;
            }
        }
    }
}