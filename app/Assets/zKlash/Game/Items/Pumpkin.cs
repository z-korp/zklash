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

        public ItemEnum Usage(Phase phase)
        {
            if (phase == Phase.OnDeath)
            {
                switch (_size)
                {
                    case Size.Small: return ItemEnum.None;
                    case Size.Medium: return ItemEnum.PumpkinSmall;
                    case Size.Large: return ItemEnum.PumpkinMedium;
                    default: return ItemEnum.None;
                }
            }
            else
            {
                switch (_size)
                {
                    case Size.Small: return ItemEnum.PumpkinSmall;
                    case Size.Medium: return ItemEnum.PumpkinMedium;
                    case Size.Large: return ItemEnum.PumpkinLarge;
                    default: return ItemEnum.None;
                }
            }
        }
    }


}