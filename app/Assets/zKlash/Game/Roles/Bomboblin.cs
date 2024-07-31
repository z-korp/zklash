namespace zKlash.Game.Roles
{
    public class Bomboblin : IRole
    {
        public Role GetRole => Role.Bomboblin;

        public int Health(Phase phase, int level)
        {
            if (phase == Phase.OnHire)
                return 1;
            return 0;
        }

        public int Attack(Phase phase, int level)
        {
            return 0;
        }

        public int Absorb(Phase phase, int level)
        {
            return 0;
        }

        public int Damage(Phase phase, int level)
        {
            if (phase == Phase.OnDeath)
                return 99;
            return 0;
        }

        public int Stun(Phase phase, int level)
        {
            return 0;
        }

        public int NextHealth(Phase phase, int level)
        {
            if (phase == Phase.OnDeath)
                return level;
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

        public int Cost(int level)
        {
            switch (level)
            {
                case 0:
                    return 0;
                case 1:
                    return 30;
                case 2:
                    return 90;
                case 3:
                    return 180;
                default:
                    return 0;
            }
        }
    }
}