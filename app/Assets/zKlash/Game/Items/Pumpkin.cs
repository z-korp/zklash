namespace zKlash.Game.Items
{
    public class Pumpkin : IItem
    {
        private Size _size;

        public Pumpkin(Size size)
        {
            _size = size;
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
    }


}