using UnityEngine;
using System.Collections.Generic;

[System.Serializable] // Makes the struct visible in the Unity Inspector, allowing for easier debugging and setup
public class ShopSpot
{
    public bool IsAvailable;
    public string EntityContained;

    public ShopSpot(bool isAvailable, string entityContained = null)
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

    public void UpdateFirstAvailableSpot(string entityName)
    {
        foreach (var spot in Spots)
        {
            if (!spot.IsAvailable && string.IsNullOrEmpty(spot.EntityContained))
            {
                spot.EntityContained = entityName; // Assign the entity to this spot
                Debug.Log($"Spot updated with entity: {entityName}");
                return; // Exit the method after updating the first matching spot
            }
        }
    }

    // Call this method to fill a spot with an entity
    public bool FillSpot(int spotIndex, string entityContained)
    {
        if (spotIndex < 0 || spotIndex >= Spots.Count || !Spots[spotIndex].IsAvailable)
        {
            return false; // Spot index is out of range or spot is not available
        }

        Spots[spotIndex] = new ShopSpot(false, entityContained); // Fill the spot
        Debug.Log($"Spot {spotIndex} filled with entity: {entityContained}");
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
        return true;
    }
}
