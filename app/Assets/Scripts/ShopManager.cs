using UnityEngine;
using zKlash.Game.Roles;
using zKlash.Game.Items;

public class ShopManager : MonoBehaviour
{
    private GameObject[] shopMobs;
    private GameObject[] shopItems;
    public static ShopManager instance;

    private bool isShopOpen = false;
    public uint rolesUint = 66051;
    public uint itemUint = 4;

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
            Role[] roles = PlayerData.Instance.SplitRoles(rolesUint);

            for (int i = 0; i < shopMobs.Length; i++)
            {
                MobSpawn spawn = shopMobs[i].GetComponent<MobSpawn>();
                spawn.SetRole(roles[i]);
            }

            ItemSpawn itemSpawn = shopItems[0].GetComponent<ItemSpawn>();
            Item item = (Item)itemUint;
            Debug.Log("qqqqqqqqqqqqqqqqqqq" + item);
            Debug.Log("qqqqqqqqqqqqqqqqqqq" + (Item)0);
            Debug.Log("qqqqqqqqqqqqqqqqqqq" + (Item)1);
            Debug.Log("qqqqqqqqqqqqqqqqqqq" + (Item)7);
            Debug.Log("qqqqqqqqqqqqqqqqqqq" + (Item)8);
            itemSpawn.SetItem((Item)itemUint);
        }
    }

    private void SetRoles(uint _roles)
    {
        rolesUint = _roles;
    }

    private void SetItem(uint _item)
    {
        itemUint = _item;
    }

    public void Reroll()
    {
        isShopOpen = false;
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