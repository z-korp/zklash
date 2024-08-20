using System.Collections.Generic;
using Dojo;
using Dojo.Starknet;
using UnityEngine;
using System.ComponentModel;
using UnityEngine.SceneManagement;
using zklash;

namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class IsExternalInit { }
}

public class GameManager : MonoBehaviour
{
    public WorldManager worldManager;

    public WorldManagerData dojoConfig;
    [SerializeField] GameManagerData gameManagerData;

    public BurnerManager burnerManager;

    private Dictionary<FieldElement, string> spawnedAccounts = new();
    public AccountSystem accountSystem;

    public JsonRpcClient provider;
    public Account masterAccount;

    public static GameManager Instance { get; private set; }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    async void Start()
    {
        Debug.Log("---------------------------------");
        Debug.Log("GameManager Start");

        try
        {
            Debug.Log("dojoConfig.rpcUrl: " + dojoConfig.rpcUrl);
            Debug.Log("dojoConfig.toriiUrl: " + dojoConfig.toriiUrl);
            Debug.Log("gameManagerData.masterPrivateKey: " + gameManagerData.masterPrivateKey);
            Debug.Log("gameManagerData.masterAddress: " + gameManagerData.masterAddress);

            provider = new JsonRpcClient(dojoConfig.rpcUrl);
            masterAccount = new Account(provider, new SigningKey(gameManagerData.masterPrivateKey), new FieldElement(gameManagerData.masterAddress));
            burnerManager = new BurnerManager(provider, masterAccount);

            if (burnerManager.Burners.Count == 0)
            {
                Debug.Log("No burners found. Deploying a new burner.");
                await burnerManager.DeployBurner();
            }

            worldManager.synchronizationMaster.OnEntitySpawned.AddListener(InitEntity);
            foreach (var entity in worldManager.Entities())
            {
                InitEntity(entity);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error in GameManager Start: " + e.Message);
        }
    }

    void Update()
    {
    }

    private void InitEntity(GameObject entity)
    {
        Debug.Log("------> InitEntity");
        Account currentBurner = burnerManager.CurrentBurner;
        if (currentBurner == null)
        {
            Debug.Log("No current burner");
            return;
        }

        Debug.Log($"wwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");
        Player playerComponent = entity.GetComponent<Player>();
        if (playerComponent != null)
        {
            Debug.Log($"-> Player entity spawned");
            Debug.Log($"playerComponent.id: {playerComponent.id.Hex()}");
            Debug.Log($"currentBurner.Address: {currentBurner.Address.Hex()}");
            if (currentBurner.Address.Hex() == playerComponent.id.Hex())
            {
                Debug.Log(">>>>>>>>>>>> Current player information stored.");
                Debug.Log($"Player entity spawned with id: {entity.name}");
                PlayerData.Instance.playerEntity = entity.name;
                var player = worldManager.Entity(entity.name).GetComponent<Player>();

                string playerName = ShortString.DecodeShortString(player.name);

                PlayerData.Instance.SetPlayerName(playerName);
                SceneManager.LoadScene("ProfileScene");
            }
        }

        Shop shopComponent = entity.GetComponent<Shop>();
        if (shopComponent != null)
        {
            Debug.Log($"-> Shop entity spawned");
            if (shopComponent.player_id.Hex() == currentBurner.Address.Hex())
            {
                Debug.Log(">>>>>>>>>>>> Current shop information stored.");
                PlayerData.Instance.shopEntity = entity.name;
                PlayerData.Instance.teamEntity = entity.name;
            }
        }

        Character characterComponent = entity.GetComponent<Character>();
        if (characterComponent != null)
        {
            Debug.Log($"-> Character entity spawned");
            if (characterComponent.player_id.Hex() == currentBurner.Address.Hex())
            {
                Debug.Log(">>>>>>>>>>>> Current character information stored.");
                PlayerData.Instance.characterEntities.Add(entity.name);
                var character = worldManager.Entity(entity.name).GetComponent<Character>();
                Debug.Log($"Character entity spawned with id: {entity.name}");
                TeamManager.Instance.PendingEntity = entity.name;
            }
        }

        /*Foe foeComponent = entity.GetComponent<Foe>();
        if (foeComponent != null)
        {
            Debug.Log($"-> Foe entity spawned");
            if (foeComponent.player_id.Hex() == currentBurner.Address.Hex())
            {
                Debug.Log(">>>>>>>>>>>> Current foe information stored.");
                PlayerData.Instance.foeEntities.Add(entity.name);
                var foe = worldManager.Entity(entity.name).GetComponent<Foe>();
                Debug.Log($"Foe entity spawned with id: {entity.name}");
                TeamManager.instance.UpdateMissingEntity(entity.name);
            }
        }*/

        /*Team teamComponent = entity.GetComponent<Team>();
        if (teamComponent != null)
        {
            Debug.Log($"-> Team entity spawned");
            if (teamComponent.player_id.Hex() == currentBurner.Address.Hex())
            {
                Debug.Log(">>>>>>>>>>>> Current team information stored.");
                PlayerData.Instance.teamEntity = entity.name;
            }
        }

        Foe foeComponent = entity.GetComponent<Foe>();
        if (foeComponent != null)
        {
            Debug.Log($"-> Foe entity spawned");
            if (foeComponent.player_id.Hex() == currentBurner.Address.Hex())
            {
                Debug.Log(">>>>>>>>>>>> Current foe information stored.");
                PlayerData.Instance.foeEntities.Add(entity.name);
                var foe = worldManager.Entity(entity.name).GetComponent<Foe>();
                Debug.Log($"Foe entity spawned with id: {entity.name}");
                TeamManager.instance.UpdateMissingEntity(entity.name);
            }
        }

        Squad squadComponent = entity.GetComponent<Squad>();
        if (squadComponent != null)
        {
            Debug.Log($"-> Squad entity spawned");
            if (squadComponent.player_id.Hex() == currentBurner.Address.Hex())
            {
                Debug.Log(">>>>>>>>>>>> Current squad information stored.");
                PlayerData.Instance.squadEntity = entity.name;
            }
        }*/

        Debug.Log($"^^^^^^^^^^^^^^^^^^^^^^^^");
    }

    public string GetSquadEntity(uint registryId, uint squadId)
    {
        foreach (var entity in worldManager.Entities())
        {
            Squad squadComponent = entity.GetComponent<Squad>();
            if (squadComponent != null)
            {
                if (squadComponent.registry_id == registryId && squadComponent.id == squadId)
                    return entity.name;
            }
        }
        return "";
    }

    public List<string> GetFoeEntities(uint registryId, uint squadId)
    {
        List<string> foeEntities = new List<string>();
        Debug.Log($"======= GetFoeEntities: registryId: {registryId}, squadId: {squadId}");
        foreach (var entity in worldManager.Entities())
        {

            Debug.Log($"Entity: {entity}");
            Foe foeComponent = entity.GetComponent<Foe>();
            if (foeComponent != null)
            {
                if (foeComponent.registry_id == registryId && foeComponent.squad_id == squadId)
                {
                    //foeEntities.Add(entity.name);
                    foeEntities.Insert(0, entity.name);
                }
            }
        }
        return foeEntities;
    }
}