namespace zKlash.Game.Roles
{
    public class Pawn : IRole
    {
        public int Health(Phase phase, int level)
        {
            if (phase == Phase.OnHire)
                return 2;  // Pawn has 2 health points when hired
            return 0;
        }

        public int Attack(Phase phase, int level)
        {
            if (phase == Phase.OnHire)
                return 1;  // Pawn has 1 attack point when hired
            return 0;
        }

        public int Absorb(Phase phase, int level)
        {
            return 0;  // Pawn does not have any absorb capability
        }

        public int Damage(Phase phase, int level)
        {
            return 0;  // Pawn does not deal additional damage
        }

        public int Stun(Phase phase, int level)
        {
            return 0;  // Pawn does not stun opponents
        }

        public int NextHealth(Phase phase, int level)
        {
            if (phase == Phase.OnDeath)
                return level;  // Returns the current level as next health on death
            return 0;
        }

        public int NextAttack(Phase phase, int level)
        {
            if (phase == Phase.OnDeath)
                return level;  // Returns the current level as next attack on death
            return 0;
        }

        public int NextAbsorb(Phase phase, int level)
        {
            return 0;  // Pawn does not have future absorb capabilities
        }
    }
}