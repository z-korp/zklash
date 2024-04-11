using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private GameObject[] shopMobs;
    private GameObject[] shopItems;
    public static ShopManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of ShopManager found!");
            return;
        }
        instance = this;
    }

    void Start() {
        shopMobs = GameObject.FindGameObjectsWithTag("ShopMob");
        shopItems = GameObject.FindGameObjectsWithTag("ShopItem");
    }

    public int IndexOfMobSpawn(MobSpawn _spawn)
    {
        Debug.Log("IndexOf");
        Debug.Log("shopMobs: " + shopMobs.Length);
        for (int i = 0; i < shopMobs.Length; i++)
        {
            MobSpawn spawn = shopMobs[i].GetComponent<MobSpawn>();
            Debug.Log("Current: " + shopMobs[i]);
            Debug.Log("Spawn: " + spawn);
            Debug.Log("Obj: " + _spawn);
            if (spawn == _spawn)
            {
                Debug.Log("Mob: " + i);
                return i;
            }
        }
        return -1;
    }

    public int IndexOfItemSpawn(ItemSpawn _spawn)
    {
        Debug.Log("IndexOf");
        for (int i = 0; i < shopItems.Length; i++)
        {
            ItemSpawn spawn = shopItems[i].GetComponent<ItemSpawn>();
            Debug.Log("Current: " + shopItems[i]);
            Debug.Log("Spawn: " + spawn);
            Debug.Log("Obj: " + _spawn);
            if (spawn == _spawn)
            {
                Debug.Log("Item: " + i);
                return i;
            }
        }
        return -1;
    }
}