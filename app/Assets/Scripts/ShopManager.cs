using UnityEngine;
using zKlash.Game.Roles;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public GameObject[] shopMobs;
    public GameObject[] shopItems;

    public List<Role> roles;

    public uint rolesUint;
    public uint itemUint;

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

        EventManager.OnShopUpdated += RefreshShop;
    }

    void OnDestroy()
    {
        EventManager.OnShopUpdated -= RefreshShop;
    }

    private void RefreshShop()
    {
        Debug.Log("QQQQQQQQQQQ -> Shop Refreshed");
        for (int i = 0; i < PlayerData.Instance.shopRoles.Length; i++)
        {
            MobSpawn spawn = shopMobs[i].GetComponent<MobSpawn>();
            Role role = PlayerData.Instance.shopRoles[i];
            spawn.SetRole(role);
            roles.Add(role);
        }

        ItemSpawn itemSpawn = shopItems[0].GetComponent<ItemSpawn>();
        itemSpawn.SetItem(PlayerData.Instance.shopItem);
    }

    public void Reroll()
    {
        if (PlayerData.Instance.Gold < PlayerData.Instance.rerollCost)
        {
            DialogueManager.Instance.ShowDialogue("Your broke mate !");
            Debug.LogWarning("Not enough gold to reroll.");
            return;
        }
        uint teamId = PlayerData.Instance.GetTeamId();
        StartCoroutine(TxCoroutines.Instance.ExecuteReroll(teamId, OnRerollComplete));
    }

    private void OnRerollComplete()
    {
        EventManager.RefreshShop();
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