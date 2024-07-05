using UnityEngine;

namespace zKlash.Game.Roles
{
    public class Knight : IRole
    {
        public Role GetRole => Role.Knight;

        public int Health(Phase phase, int level)
        {
            Debug.Log("Knight Health: " + phase + " " + level);
            switch (phase)
            {
                case Phase.OnHire:
                    return 3;
                case Phase.OnEquip:
                    return level;
                case Phase.OnUnequip:
                    return -level;
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