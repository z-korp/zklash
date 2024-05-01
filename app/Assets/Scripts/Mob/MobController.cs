using UnityEngine;
using zKlash.Game;
using zKlash.Game.Roles;
using zKlash.Game.Items;
using GameCharacter = zKlash.Game.Character;

public class MobController : MonoBehaviour
{
    public GameCharacter Character { get; private set; }

    void Start()
    {
    }

    public void ConfigureCharacter(Role role, int level, Item item, int xp = 0)
    {
        Character = new GameCharacter(role, level, item, xp);
    }


    public void ConfigureCharacter(GameCharacter character)
    {
        Character = character;
    }

}
