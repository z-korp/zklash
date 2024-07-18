using System.Collections.Generic;
using UnityEngine;
using zKlash.Game;
using zKlash.Game.Items;
using zKlash.Game.Roles;

[System.Serializable]
public class BattleSetup
{
    public List<CharacterSetup> alliesSetup;
    public List<CharacterSetup> enemiesSetup;
}

public static class BattleSetups
{
    public static List<BattleSetup> setups = new List<BattleSetup>
    {
        new BattleSetup // O -> bush
        {
            alliesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Pawn, level = 1, item = Item.BushMedium },
            },
            enemiesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Torchoblin, level = 1, item = Item.None },
            }
        }, // Result: LOST
        new BattleSetup // 1 -> rock
        {
            alliesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Pawn, level = 1, item = Item.RockSmall },
            },
            enemiesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Torchoblin, level = 1, item = Item.RockSmall },
            }
        },
        new BattleSetup // 2 -> shroom
        {
            alliesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Pawn, level = 1, item = Item.MushroomSmall },
            },
            enemiesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Torchoblin, level = 1, item = Item.None },
            }
        },
        new BattleSetup // 3 -> pumpkin
        {
            alliesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Pawn, level = 1, item = Item.PumpkinMedium },
            },
            enemiesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Dynamoblin, level = 1, item = Item.None },
            }
        },
        new BattleSetup // 4 -> Test Fighter Pawn talent
        {
            alliesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Pawn, level = 2, item = Item.None },
                new CharacterSetup { role = Role.Knight, level = 1, item = Item.None },
            },
            enemiesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Torchoblin, level = 1, item = Item.None },
            }
        }, // -> should win
        new BattleSetup // 5 -> Test Dual stun
        {
            alliesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Bowman, level = 2, item = Item.PumpkinSmall },
                new CharacterSetup { role = Role.Pawn, level = 1, item = Item.None },
            },
            enemiesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Bowman, level = 2, item = Item.PumpkinSmall },
            }
        }, // -> should win
        new BattleSetup // 6 -> Test Dual stun reverse
        {
            alliesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Bowman, level = 2, item = Item.PumpkinSmall },
            },
            enemiesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Bowman, level = 2, item = Item.PumpkinSmall },
                new CharacterSetup { role = Role.Pawn, level = 1, item = Item.None },
            }
        }, // -> should loose
        new BattleSetup // 7 -> Test Knight with stone
        {
            alliesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Knight, level = 1, item = Item.RockLarge },
                new CharacterSetup { role = Role.Knight, level = 1, item = Item.None },
                new CharacterSetup { role = Role.Knight, level = 1, item = Item.None },
            },
            enemiesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Dynamoblin, level = 1, item = Item.None },
                new CharacterSetup { role = Role.Bomboblin, level = 1, item = Item.None },
            }
        }, // -> should loose
        new BattleSetup // 8 -> Test case wrong win/loose result 1
        {
            alliesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Pawn, level = 3, item = Item.MushroomLarge },
                new CharacterSetup { role = Role.Knight, level = 3, item = Item.PumpkinMedium },
                new CharacterSetup { role = Role.Pawn, level = 3, item = Item.MushroomLarge },
                new CharacterSetup { role = Role.Knight, level = 3, item = Item.PumpkinMedium },
            },
            enemiesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Bomboblin, level = 3, item = Item.BushLarge },
                new CharacterSetup { role = Role.Torchoblin, level = 3, item = Item.MushroomMedium },
                new CharacterSetup { role = Role.Dynamoblin, level = 3, item = Item.RockMedium },
                new CharacterSetup { role = Role.Dynamoblin, level = 3, item = Item.PumpkinMedium },
            }
        }, // -> should ???
        new BattleSetup // 9 -> Bomboblin
        {
            alliesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Pawn, level = 1, item = Item.None },
            },
            enemiesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Bomboblin, level = 1, item = Item.None },
            }
        },
        new BattleSetup // 10 -> Torchoblin
        {
            alliesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Pawn, level = 1, item = Item.RockLarge },
            },
            enemiesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Torchoblin, level = 1, item = Item.None },
                new CharacterSetup { role = Role.Pawn, level = 1, item = Item.None },
            }
        },
        new BattleSetup // 11 -> Dynamoblin
        {
            alliesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Knight, level = 1, item = Item.RockLarge },
            },
            enemiesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Dynamoblin, level = 1, item = Item.None },
            }
        },
        new BattleSetup // 12 -> Double Dynamoblin, First kill second who kill first with death damage
        {
            alliesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Dynamoblin, level = 1, item = Item.BushLarge },
            },
            enemiesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Dynamoblin, level = 3, item = Item.None },
            }
        },
        new BattleSetup // 13 -> Double Dynamoblin, reverse than previous one
        {
            alliesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Dynamoblin, level = 3, item = Item.None },
            },
            enemiesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Dynamoblin, level = 3, item = Item.BushLarge },
            }
        },
        new BattleSetup // 14 -> Test Bowman
        {
            alliesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Bowman, level = 2, item = Item.None },
                new CharacterSetup { role = Role.Pawn, level = 1, item = Item.None },
            },
            enemiesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Torchoblin, level = 1, item = Item.MushroomLarge },
            }
        },
        new BattleSetup // 14 -> Test Pawn
        {
            alliesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Pawn, level = 1, item = Item.None },
                new CharacterSetup { role = Role.Pawn, level = 1, item = Item.None },
            },
            enemiesSetup = new List<CharacterSetup>
            {
                new CharacterSetup { role = Role.Torchoblin, level = 1, item = Item.MushroomLarge },
            }
        },
    };
}