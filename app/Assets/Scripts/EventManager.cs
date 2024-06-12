using System;
using UnityEngine;
public static class EventManager
{
    public static event Action OnStartBattle;
    public static void StartBattle()
    {
        Debug.Log("[[[[ StartBattle ]]]]");
        OnStartBattle?.Invoke();
    }

    public static event Action OnRefreshShop;
    public static void RefreshShop()
    {
        Debug.Log("[[[[ RefreshShop ]]]]");
        OnRefreshShop?.Invoke();
    }

    public static event Action OnShopUpdated;
    public static void ShopUpdated()
    {
        Debug.Log("[[[[ ShopUpdated ]]]]");
        OnShopUpdated?.Invoke();
    }

    public static event Action OnRefreshPlayerStats;
    public static void RefreshPlayerStats()
    {
        Debug.Log("[[[[ RefreshPlayerStats ]]]]");
        OnRefreshPlayerStats?.Invoke();
    }
}