using UnityEngine;
using zKlash.Game;
using zKlash.Game.Items;
using zKlash.Game.Roles;
using GameCharacter = zKlash.Game.Character;

public class MobManager : MonoBehaviour
{
    public GameObject[] prefabs;
    public static MobManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of MobFactory found!");
            return;
        }
        instance = this;
    }

    public GameObject Create(Role role, Transform transform)
    {
        string title = PrefabMappings.NameToRoleMap[role];
        GameObject prefab = PrefabUtils.FindPrefabByName(prefabs, title);
        GameObject mobObject = Instantiate(prefab, transform.position, Quaternion.identity);

        GameCharacter character = new GameCharacter(role, 1, Item.None);
        mobObject.GetComponent<MobController>().ConfigureCharacter(character);

        return mobObject;
    }
}