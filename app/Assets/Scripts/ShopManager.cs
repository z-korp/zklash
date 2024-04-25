using UnityEngine;
using zKlash.Game.Roles;
using zKlash.Game.Items;

public class ShopManager : MonoBehaviour
{
    private GameObject[] shopMobs;
    private GameObject[] shopItems;
    public static ShopManager instance;

    private bool isShopOpen = false;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of ShopManager found!");
            return;
        }
        instance = this;
    }

    void Start()
    {
        shopMobs = GameObject.FindGameObjectsWithTag("ShopMob");
        shopItems = GameObject.FindGameObjectsWithTag("ShopItem");
    }

    void Update()
    {
        /*if (PlayerData.Instance.isShopSet && !isShopOpen)
        {
            isShopOpen = true;
            for (int i = 0; i < PlayerData.Instance.shopRoles.Length; i++)
            {
                MobSpawn spawn = shopMobs[i].GetComponent<MobSpawn>();
                spawn.SetRole(PlayerData.Instance.shopRoles[i]);
            }

            ItemSpawn itemSpawn = shopItems[0].GetComponent<ItemSpawn>();
            itemSpawn.SetItem(PlayerData.Instance.shopItem);
        }*/

        // This is to avoid contract init
        if (!isShopOpen)
        {
            isShopOpen = true;
            Role[] roles = PlayerData.Instance.SplitRoles(66051);

            for (int i = 0; i < shopMobs.Length; i++)
            {
                MobSpawn spawn = shopMobs[i].GetComponent<MobSpawn>();
                spawn.SetRole(roles[i]);
            }

            ItemSpawn itemSpawn = shopItems[0].GetComponent<ItemSpawn>();
            itemSpawn.SetItem((Item)8);
        }
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