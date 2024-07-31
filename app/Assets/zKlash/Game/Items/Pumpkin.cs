namespace zKlash.Game.Items
{
    public class Pumpkin : IItem
    {
        private Size _size;

        public Pumpkin(Size size)
        {
            _size = size;
        }

        public Item GetItemType()
        {
            if (_size == Size.Small)
            {
                return Item.PumpkinSmall;
            }
            else if (_size == Size.Medium)
            {
                return Item.PumpkinMedium;
            }
            else if (_size == Size.Large)
            {
                return Item.PumpkinLarge;
            }
            else
            {
                return Item.None;
            }
        }

        public int Health(Phase phase)
        {
            if (phase == Phase.OnDeath)
                return 1;
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
            if (phase == Phase.OnDeath)
            {
                switch (_size)
                {
                    case Size.Small: return Item.None;
                    case Size.Medium: return Item.PumpkinSmall;
                    case Size.Large: return Item.PumpkinMedium;
                    default: return Item.None;
                }
            }
            else
            {
                switch (_size)
                {
                    case Size.Small: return Item.PumpkinSmall;
                    case Size.Medium: return Item.PumpkinMedium;
                    case Size.Large: return Item.PumpkinLarge;
                    default: return Item.None;
                }
            }
        }

        public int Cost()
        {
            return _size switch
            {
                Size.Small => 30,
                Size.Medium => 60,
                Size.Large => 90,
                _ => 0
            };
        }
    }


}