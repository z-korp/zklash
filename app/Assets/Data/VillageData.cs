using UnityEngine;
using System.Collections.Generic;
using zKlash.Game.Roles;

[System.Serializable] // Makes the struct visible in the Unity Inspector, allowing for easier debugging and setup
public class ShopSpot
{
    public bool IsAvailable;
    public Role EntityContained;

    public ShopSpot(bool isAvailable, Role entityContained = Role.None)
    {
        IsAvailable = isAvailable;
        EntityContained = entityContained;
    }
}

public class VillageData : MonoBehaviour
{
    // Static instance of VillageData which allows it to be accessed by any other script.
    private static VillageData _instance;

    // Public property to access instance
    public static VillageData Instance
    {
        get
        {
            if (_instance == null)
            {
                // This will only occur if no other VillageData instance is found in the scene so it creates one.
                _instance = FindObjectOfType<VillageData>();
                if (_instance == null)
                {
                    // Creates a new GameObject with VillageData component if none exist in the scene.
                    GameObject go = new GameObject("VillageData");
                    _instance = go.AddComponent<VillageData>();
                }
            }
            return _instance;
        }
    }

    public List<ShopSpot> Spots = new List<ShopSpot>();

    public List<Hit> hitEventDetails = new List<Hit>();
    public List<Fighter> fighterEventDetails = new List<Fighter>();
    public List<Stun> stunEventDetails = new List<Stun>();
    public List<Absorb> absorbEventDetails = new List<Absorb>();
    public List<Usage> usageEventDetails = new List<Usage>();
    public List<Talent> talentEventDetails = new List<Talent>();

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

        // Initialize shop spots
        if (Spots.Count == 0) // Initialize only if not already initialized
        {
            for (int i = 0; i < 4; i++) // Assuming 4 spots
            {
                Spots.Add(new ShopSpot(true)); // Initially, all spots are available
            }
        }
    }

    public void UpdateFirstAvailableSpot(Role _role)
    {
        foreach (var spot in Spots)
        {
            if (!spot.IsAvailable && spot.EntityContained == Role.None)
            {
                spot.EntityContained = _role; // Assign the entity to this spot
                Debug.Log($"Spot updated with entity: {_role}");
                return; // Exit the method after updating the first matching spot
            }
        }
    }

    // Call this method to fill a spot with an entity
    public bool FillSpot(int spotIndex, Role _role)
    {
        if (spotIndex < 0 || spotIndex >= Spots.Count || !Spots[spotIndex].IsAvailable)
        {
            return false; // Spot index is out of range or spot is not available
        }

        Spots[spotIndex] = new ShopSpot(false, _role); // Fill the spot
        Debug.Log($"Spot {spotIndex} filled with entity: {_role}");
        return true;
    }

    // Call this method to free up a spot
    public bool FreeSpot(int spotIndex)
    {
        if (spotIndex < 0 || spotIndex >= Spots.Count)
        {
            return false; // Spot index is out of range
        }

        Spots[spotIndex] = new ShopSpot(true); // Make the spot available again
        Debug.Log($"Spot {spotIndex} is now available");
        return true;
    }

    public Role RoleAtIndex(int index)
    {
        if (index < 0 || index >= Spots.Count)
        {
            return Role.None; // Return Role.None if index is out of range
        }

        return Spots[index].EntityContained; // Return the entity contained in the spot
    }
}
