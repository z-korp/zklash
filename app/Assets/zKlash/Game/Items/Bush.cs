namespace zKlash.Game.Items
{
    public class Bush : IItem
    {
        private Size _size;

        public Bush(Size size)
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

        public ItemEnum Usage(Phase phase)
        {
            return _size switch
            {
                Size.Small => ItemEnum.BushSmall,
                Size.Medium => ItemEnum.BushMedium,
                Size.Large => ItemEnum.BushLarge,
                _ => ItemEnum.None
            };
        }
    }
}