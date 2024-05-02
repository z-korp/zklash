using UnityEngine;
using zKlash.Game.Roles;
using zKlash.Game.Items;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public GameObject[] shopMobs;
    public GameObject[] shopItems;

    public uint rolesUint;
    public uint itemUint;

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
        rolesUint = 66051;
        itemUint = 1;
    }

    void Update()
    {
        if (PlayerData.Instance.isShopSet && !isShopOpen)
        {
            isShopOpen = true;
            for (int i = 0; i < PlayerData.Instance.shopRoles.Length; i++)
            {
                MobSpawn spawn = shopMobs[i].GetComponent<MobSpawn>();
                spawn.SetRole(PlayerData.Instance.shopRoles[i]);
            }

            ItemSpawn itemSpawn = shopItems[0].GetComponent<ItemSpawn>();
            itemSpawn.SetItem(PlayerData.Instance.shopItem);
        }

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
            itemSpawn.SetItem((Item)itemUint);
        }
    }

    public void Reroll()
    {
        if (PlayerData.Instance.Gold < PlayerData.Instance.rerollCost)
        {
            Debug.LogWarning("Not enough gold to reroll.");
            return;
        }
        uint teamId = PlayerData.Instance.GetTeamId();
        StartCoroutine(TxCoroutines.Instance.ExecuteReroll(teamId, OnRerollComplete));
    }

    private void OnRerollComplete()
    {
        isShopOpen = false;
        PlayerData.Instance.isShopSet = false;
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