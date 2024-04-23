namespace zKlash.Game
{
    public struct Buff
    {
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Absorb { get; set; }
    }

    public enum Phase
    {
        OnHire,
        OnEquip,
        OnDispatch,
        OnFight,
        OnDeath,
    }
}