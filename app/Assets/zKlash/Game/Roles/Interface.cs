using System;

namespace zKlash.Game.Roles
{

    public enum Role
    {
        None, Knight, Bowman, Pawn, Torchoblin, Dynamoblin, Bomboblin,
    }

    public interface IRole
    {
        Role GetRole { get; }
        int Health(Phase phase, int level);
        int Attack(Phase phase, int level);
        int Absorb(Phase phase, int level);
        int Damage(Phase phase, int level);
        int Stun(Phase phase, int level);
        int NextHealth(Phase phase, int level);
        int NextAttack(Phase phase, int level);
        int NextAbsorb(Phase phase, int level);
    }

    public class NoneRole : IRole
    {
        public Role GetRole => Role.None;
        public int Health(Phase phase, int level) => 0;
        public int Attack(Phase phase, int level) => 0;
        public int Absorb(Phase phase, int level) => 0;
        public int Damage(Phase phase, int level) => 0;
        public int Stun(Phase phase, int level) => 0;
        public int NextHealth(Phase phase, int level) => 0;
        public int NextAttack(Phase phase, int level) => 0;
        public int NextAbsorb(Phase phase, int level) => 0;
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
                case Role.None:
                    return new NoneRole();
                default:
                    throw new ArgumentException("Invalid role type");
            }
        }
    }

}