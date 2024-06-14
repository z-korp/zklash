using UnityEngine;
using zKlash.Game.Items;

public class ItemManager : MonoBehaviour
{
    public GameObject[] prefabs;
    public static ItemManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of ItemManager found!");
            return;
        }
        instance = this;
    }

    public GameObject Create(Item item, Transform transform)
    {
        string title = PrefabMappings.NameToItemMap[item];
        GameObject prefab = PrefabUtils.FindPrefabByName(prefabs, title);
        return Instantiate(prefab, transform.position, Quaternion.identity);
    }
}