namespace zKlash.Game.Items
{
    public class Mushroom : IItem
    {
        private Size _size;

        public Mushroom(Size size)
        {
            _size = size;
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

        public ItemEnum Usage(Phase phase)
        {
            switch (_size)
            {
                case Size.Small: return ItemEnum.MushroomSmall;
                case Size.Medium: return ItemEnum.MushroomMedium;
                case Size.Large: return ItemEnum.BushLarge;
                default: return ItemEnum.None;
            }
        }
    }
}