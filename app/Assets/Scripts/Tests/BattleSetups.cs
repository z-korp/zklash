using System.Collections.Generic;
using UnityEngine;
using zKlash.Game;
using zKlash.Game.Items;
using zKlash.Game.Roles;

[System.Serializable]
public class BattleSetup
{
    public List<CharacterSetupTest> alliesSetup;
    public List<CharacterSetupTest> enemiesSetup;
}

public static class BattleSetups
{
    public static List<BattleSetup> setups = new List<BattleSetup>
    {
        new BattleSetup // O -> bush
        {
            alliesSetup = new List<CharacterSetupTest>
            {
                new CharacterSetupTest { role = Role.Pawn, level = 1, item = Item.BushMedium },
            },
            enemiesSetup = new List<CharacterSetupTest>
            {
                new CharacterSetupTest { role = Role.Torchoblin, level = 1, item = Item.None },
            }
        }, // Result: LOST
        new BattleSetup // 1 -> rock
        {
            alliesSetup = new List<CharacterSetupTest>
            {
                new CharacterSetupTest { role = Role.Pawn, level = 1, item = Item.RockSmall },
            },
            enemiesSetup = new List<CharacterSetupTest>
            {
                new CharacterSetupTest { role = Role.Torchoblin, level = 1, item = Item.None },
            }
        },
        new BattleSetup // 2 -> shroom
        {
            alliesSetup = new List<CharacterSetupTest>
            {
                new CharacterSetupTest { role = Role.Pawn, level = 1, item = Item.MushroomSmall },
            },
            enemiesSetup = new List<CharacterSetupTest>
            {
                new CharacterSetupTest { role = Role.Torchoblin, level = 1, item = Item.None },
            }
        },
        new BattleSetup // 3 -> pumpkin
        {
            alliesSetup = new List<CharacterSetupTest>
            {
                new CharacterSetupTest { role = Role.Pawn, level = 1, item = Item.PumpkinMedium },
            },
            enemiesSetup = new List<CharacterSetupTest>
            {
                new CharacterSetupTest { role = Role.Dynamoblin, level = 1, item = Item.None },
            }
        },
        new BattleSetup // 4 -> Test Fighter Pawn talent
        {
            alliesSetup = new List<CharacterSetupTest>
            {
                new CharacterSetupTest { role = Role.Pawn, level = 2, item = Item.None },
                new CharacterSetupTest { role = Role.Knight, level = 1, item = Item.None },
            },
            enemiesSetup = new List<CharacterSetupTest>
            {
                new CharacterSetupTest { role = Role.Torchoblin, level = 1, item = Item.None },
            }
        }, // -> should win
        new BattleSetup // 5 -> Test Dual stun
        {
            alliesSetup = new List<CharacterSetupTest>
            {
                new CharacterSetupTest { role = Role.Bowman, level = 2, item = Item.PumpkinSmall },
                new CharacterSetupTest { role = Role.Pawn, level = 1, item = Item.None },
            },
            enemiesSetup = new List<CharacterSetupTest>
            {
                new CharacterSetupTest { role = Role.Bowman, level = 2, item = Item.PumpkinSmall },
            }
        }, // -> should win
        new BattleSetup // 6 -> Test Dual stun reverse
        {
            alliesSetup = new List<CharacterSetupTest>
            {
                new CharacterSetupTest { role = Role.Bowman, level = 2, item = Item.PumpkinSmall },
            },
            enemiesSetup = new List<CharacterSetupTest>
            {
                new CharacterSetupTest { role = Role.Bowman, level = 2, item = Item.PumpkinSmall },
                new CharacterSetupTest { role = Role.Pawn, level = 1, item = Item.None },
            }
        }, // -> should loose
        new BattleSetup // 7 -> Test Knight with stone
        {
            alliesSetup = new List<CharacterSetupTest>
            {
                new CharacterSetupTest { role = Role.Knight, level = 1, item = Item.RockLarge },
                new CharacterSetupTest { role = Role.Knight, level = 1, item = Item.None },
                new CharacterSetupTest { role = Role.Knight, level = 1, item = Item.None },
            },
            enemiesSetup = new List<CharacterSetupTest>
            {
                new CharacterSetupTest { role = Role.Dynamoblin, level = 1, item = Item.None },
                new CharacterSetupTest { role = Role.Bomboblin, level = 1, item = Item.None },
            }
        }, // -> should loose
        
    };
}