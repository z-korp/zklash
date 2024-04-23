using System;

namespace zKlash.Game.Roles
{

    public enum Role
    {
        None, Knight, Bowman, Pawn, Torchoblin, Dynamoblin, Bomboblin,
    }

    public interface IRole
    {
        int Health(Phase phase, int level);
        int Attack(Phase phase, int level);
        int Absorb(Phase phase, int level);
        int Damage(Phase phase, int level);
        int Stun(Phase phase, int level);
        int NextHealth(Phase phase, int level);
        int NextAttack(Phase phase, int level);
        int NextAbsorb(Phase phase, int level);
    }

    public static class RoleFactory
    {
        public static IRole GetRole(Role role)
        {
            switch (role)
            {
                case Role.Knight:
                    return new Knight();
                case Role.Bowman:
                    return new Bowman();
                case Role.Pawn:
                    return new Pawn();
                case Role.Torchoblin:
                    return new Torchoblin();
                case Role.Dynamoblin:
                    return new Dynamoblin();
                case Role.Bomboblin:
                    return new Bomboblin();
                default:
                    throw new ArgumentException("Invalid role type");
            }
        }
    }

}