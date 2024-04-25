namespace zKlash.Game.Items
{
    public class Rock : IItem
    {
        private Size _size;

        public Rock(Size size)
        {
            _size = size;
        }

        public int Health(Phase phase)
        {
            return 0;
        }

        public int Attack(Phase phase)
        {
            return 0;
        }

        public int Damage(Phase phase)
        {
            if (phase == Phase.OnFight)
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

        public int Absorb(Phase phase)
        {
            return 0;
        }

        public Item Usage(Phase phase)
        {
            if (phase == Phase.OnFight)
            {
                return Item.None;
            }
            else
            {
                return _size switch
                {
                    Size.Small => Item.RockSmall,
                    Size.Medium => Item.RockMedium,
                    Size.Large => Item.RockLarge,
                    _ => Item.None
                };
            }
        }
    }
}