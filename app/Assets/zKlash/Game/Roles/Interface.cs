using System;

namespace zKlash.Game.Roles
{

    public enum RoleEnum
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
        public static IRole GetRole(RoleEnum role)
        {
            switch (role)
            {
                case RoleEnum.Knight:
                    return new Knight();
                case RoleEnum.Bowman:
                    return new Bowman();
                case RoleEnum.Pawn:
                    return new Pawn();
                case RoleEnum.Torchoblin:
                    return new Torchoblin();
                case RoleEnum.Dynamoblin:
                    return new Dynamoblin();
                case RoleEnum.Bomboblin:
                    return new Bomboblin();
                default:
                    throw new ArgumentException("Invalid role type");
            }
        }
    }

}