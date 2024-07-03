namespace zKlash.Game.Roles
{
    public class Knight : IRole
    {

        public Role GetRole => Role.Knight;
        private bool _firstTimeInOnUnequip = true;

        public int Health(Phase phase, int level)
        {
            switch (phase)
            {
                case Phase.OnHire:
                    return 3;
                case Phase.OnEquip:
                    return level;
                case Phase.OnUnequip:
                    if (_firstTimeInOnUnequip)
                    {
                        _firstTimeInOnUnequip = false;
                        return 0;
                    }
                    else
                    {
                        return -level;
                    }
                default:
                    return 0;
            }
        }

        public int Attack(Phase phase, int level)
        {
            if (phase == Phase.OnHire)
                return 1;
            return 0;
        }

        public int Absorb(Phase phase, int level)
        {
            return 0;
        }

        public int Damage(Phase phase, int level)
        {
            return 0;
        }

        public int Stun(Phase phase, int level)
        {
            return 0;
        }

        public int NextHealth(Phase phase, int level)
        {
            return 0;
        }

        public int NextAttack(Phase phase, int level)
        {
            return 0;
        }

        public int NextAbsorb(Phase phase, int level)
        {
            return 0;
        }
    }
}