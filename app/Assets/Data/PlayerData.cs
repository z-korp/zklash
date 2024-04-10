using UnityEngine;
using System.Collections.Generic;

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
}
