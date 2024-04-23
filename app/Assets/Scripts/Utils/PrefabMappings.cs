using System.Collections.Generic;
using UnityEngine;
using zKlash.Game.Roles;
using zKlash.Game.Items;

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

    public static readonly Dictionary<Item, string> NameToItemDataMap = new Dictionary<Item, string>
    {
        { Item.None, "None"},
        { Item.BushSmall, "BushS" },
        { Item.BushMedium, "BushM" },
        { Item.BushLarge, "BushL" },
        { Item.MushroomSmall, "ShroomS" },
        { Item.MushroomMedium, "ShroomM" },
        { Item.MushroomLarge, "ShroomL" },
        { Item.PumpkinSmall, "PumpkinS" },
        { Item.PumpkinMedium, "PumpkinM" },
        { Item.PumpkinLarge, "PumpkinL" },
        { Item.RockSmall, "RockS" },
        { Item.RockMedium, "RockM" },
        { Item.RockLarge, "RockL" },
    };
}
