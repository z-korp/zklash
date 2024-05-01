using UnityEngine;
using System.Collections.Generic;
using zKlash.Game.Roles;
using System;
using zKlash.Game.Items;

public class PlayerData : MonoBehaviour
{
    // Static instance of PlayerData which allows it to be accessed by any other script.
    private static PlayerData _instance;

    // Public property to access instance
    public static PlayerData Instance
    {
        get
        {
            if (_instance == null)
            {
                // This will only occur if no other PlayerData instance is found in the scene so it creates one.
                _instance = FindObjectOfType<PlayerData>();
                if (_instance == null)
                {
                    // Creates a new GameObject with PlayerData component if none exist in the scene.
                    GameObject go = new GameObject("PlayerData");
                    _instance = go.AddComponent<PlayerData>();
                }
            }
            return _instance;
        }
    }

    public string address;
    public string playerEntity;
    public string shopEntity;
    public string teamEntity;
    public List<string> characterEntities = new List<string>();

    public bool isShopSet = false;
    public Role[] shopRoles = new Role[3];

    public Item shopItem = Item.None;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Keeps the instance alive across the entire game
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // Destroys duplicate instances
        }
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(shopEntity) && !isShopSet)
        {
            var shop = GameManager.Instance.worldManager.Entity(shopEntity).GetComponent<Shop>();

            // Roles
            Role[] roles = SplitRoles(shop.roles);
            for (int i = 0; i < shopRoles.Length; i++)
            {
                shopRoles[i] = roles[i];
            }

            // Item
            shopItem = (Item)shop.items;

            isShopSet = true;
        }
    }

    public Role[] SplitRoles(uint roles)
    {
        string hexStr = roles.ToString("X6");
        List<Role> roleList = new List<Role>();

        for (int i = 0; i < hexStr.Length; i += 2)
        {
            // Extract two characters at a time
            string hexPart = hexStr.Substring(i, 2);

            // Convert hex to uint
            uint decimalValue = Convert.ToUInt32(hexPart, 16);

            if (Enum.IsDefined(typeof(Role), (int)decimalValue))
            {
                roleList.Add((Role)decimalValue);
            }
            else
            {
                roleList.Add(Role.None);
            }
        }

        return roleList.ToArray();
    }

    public uint GetTeamId()
    {
        if (string.IsNullOrEmpty(teamEntity))
        {
            Debug.LogError("Team entity not set");
            return 0;
        }
        var team = GameManager.Instance.worldManager.Entity(teamEntity).GetComponent<Team>();
        return team.id;
    }
}
