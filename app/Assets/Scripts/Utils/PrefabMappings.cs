using System.Collections.Generic;
using UnityEngine;

public enum Role
{
    None, Knight, Bowman, Pawn, Torchoblin, Dynamoblin, Bomboblin,
}

public enum Item
{
    None, MushroomSmall, MushroomMedium, MushroomLarge, RockSmall, RockMedium, RockLarge, BushSmall, BushMedium, BushLarge, PumpkinSmall, PumpkinMedium, PumpkinLarge,
}

public class PrefabMappings
{
    public static readonly Dictionary<Role, string> NameToRoleMap = new Dictionary<Role, string>
    {
        { Role.Knight, "Warrior" },
        { Role.Bowman, "Archer" },
        { Role.Pawn, "Pawn" },
        { Role.Torchoblin, "Torchoblin" },
        { Role.Dynamoblin, "Dynamoblin" },
        { Role.Bomboblin, "Bomboblin" },
    };

    public static readonly Dictionary<Item, string> NameToItemMap = new Dictionary<Item, string>
    {
        { Item.BushSmall, "Bush1" },
        { Item.BushMedium, "Bush2" },
        { Item.BushLarge, "Bush3" },
        { Item.MushroomSmall, "Mushroom1" },
        { Item.MushroomMedium, "Mushroom2" },
        { Item.MushroomLarge, "Mushroom3" },
        { Item.PumpkinSmall, "Pumpkin1" },
        { Item.PumpkinMedium, "Pumpkin2" },
        { Item.PumpkinLarge, "Pumpkin3" },
        { Item.RockSmall, "Rock1" },
        { Item.RockMedium, "Rock2" },
        { Item.RockLarge, "Rock3" },
    };
}
