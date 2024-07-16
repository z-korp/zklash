using System.Collections.Generic;
using Dojo;
using Dojo.Starknet;
using UnityEngine;


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
        provider = new JsonRpcClient(dojoConfig.rpcUrl);
        var signer = new SigningKey(gameManagerData.masterPrivateKey);
        masterAccount = new Account(provider, signer, new FieldElement(gameManagerData.masterAddress));

        burnerManager = new BurnerManager(provider, masterAccount);

        var burner = await burnerManager.DeployBurner(new SigningKey());

        worldManager.synchronizationMaster.OnEntitySpawned.AddListener(InitEntity);
        foreach (var entity in worldManager.Entities())
        {
            InitEntity(entity);
        }



        //worldManager.synchronizationMaster.OnSynchronized.AddListener(InitEntity2);
    }

    void Update()
    {

    }

    /*private void InitEntity2(List<GameObject> entities)
    {
        Debug.Log($"---------------------------------");
        foreach (var entity in entities)
        {
            Debug.Log($"Entity spawned with id: {entity.name}");
            //InitEntity(entity);
        }
        Debug.Log($"---------------------------------");
    }*/

    private void InitEntity(GameObject entity)
    {
        Account currentBurner = burnerManager.CurrentBurner;
        if (currentBurner == null)
        {
            Debug.Log("No current burner");
            return;
        }

        Debug.Log($"---------------------------------");
        Debug.Log($"currentBurner: {currentBurner.Address.Hex()}");
        Debug.Log($"Entity spawned with id: {entity.name}");

        Player playerComponent = entity.GetComponent<Player>();
        if (playerComponent != null)
        {
            Debug.Log($"-> Player entity spawned");
            if (currentBurner.Address.Hex() == playerComponent.id.Hex())
            {
                Debug.Log(">>>>>>>>>>>> Current player information stored.");
                Debug.Log($"Player entity spawned with id: {entity.name}");
                PlayerData.Instance.playerEntity = entity.name;
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

        Debug.Log($"---------------------------------");
    }

    // not working in webgl
    /*public async void TriggerCreateAndSpawnAsync(string name)
    {
        Debug.Log("TriggerCreateAndSpawnAsync");
        Account currentBurner = burnerManager.CurrentBurner;
        var nameHex = StringToHexString(name);

        try
        {
            var txHash = await accountSystem.Create(currentBurner, dojoConfig.worldAddress, nameHex);
            Debug.Log($"[Create] Transaction Hash: {txHash.Hex()}");
            await provider.WaitForTransaction(txHash);
            //await Task.Delay(500);

            txHash = await accountSystem.Spawn(currentBurner, dojoConfig.worldAddress);
            Debug.Log($"[Spawn] Transaction Hash: {txHash.Hex()}");
            await provider.WaitForTransaction(txHash);
            //await Task.Delay(500);
        }
        catch (Exception e)
        {
            Debug.Log($"Error: {e.Message}");
        }
    }*/

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
        foreach (var entity in worldManager.Entities())
        {
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