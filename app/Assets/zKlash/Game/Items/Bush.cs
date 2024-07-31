namespace zKlash.Game.Items
{
    public class Bush : IItem
    {
        private Size _size;

        public Bush(Size size)
        {
            _size = size;
        }

        public Item GetItemType()
        {
            if (_size == Size.Small)
            {
                return Item.BushSmall;
            }
            else if (_size == Size.Medium)
            {
                return Item.BushMedium;
            }
            else if (_size == Size.Large)
            {
                return Item.BushLarge;
            }
            else
            {
                return Item.None;
            }
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
            return 0;
        }

        public int Absorb(Phase phase)
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

        public Item Usage(Phase phase)
        {
            return Item.None; // only one-time use
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