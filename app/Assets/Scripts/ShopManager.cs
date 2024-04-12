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
        for (int i = 0; i < shopMobs.Length; i++)
        {
            MobSpawn spawn = shopMobs[i].GetComponent<MobSpawn>();
            if (spawn == _spawn)
            {
                return i;
            }
        }
        return -1;
    }

    public int IndexOfItemSpawn(ItemSpawn _spawn)
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            ItemSpawn spawn = shopItems[i].GetComponent<ItemSpawn>();
            if (spawn == _spawn)
            {
                return i;
            }
        }
        return -1;
    }
}