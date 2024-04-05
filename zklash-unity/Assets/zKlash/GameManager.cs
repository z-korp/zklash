using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using bottlenoselabs.C2CS.Runtime;
using Dojo;
using Dojo.Starknet;
using dojo_bindings;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = System.Object;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] WorldManager worldManager;

    [SerializeField] WorldManagerData dojoConfig;
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

    public static string StringToHexString(string input)
    {
        string hexOutput = "";
        foreach (char c in input)
        {
            hexOutput += String.Format("{0:X2}", (int)c);
        }
        return hexOutput;
    }

    
    void Start()
    {
        Debug.Log("---------------------------------");
        Debug.Log("GameManager Start");
        provider = new JsonRpcClient(dojoConfig.rpcUrl);
        var signer = new SigningKey(gameManagerData.masterPrivateKey);
        masterAccount = new Account(provider, signer, new FieldElement(gameManagerData.masterAddress));

        burnerManager = new BurnerManager(provider, masterAccount);

        worldManager.synchronizationMaster.OnEntitySpawned.AddListener(InitEntity);
        foreach (var entity in worldManager.Entities())
        {
            InitEntity(entity);
        }
    }

    async void Update()
    {
        
    }

    private void InitEntity(GameObject entity)
    {
        /*Game gameComponent = entity.GetComponent<Game>();
        if (gameComponent != null)
        {
            // This entity is of type Tile, perform actions with its index
            Debug.Log($"Game entity spawned with id: {gameComponent.game_id}");
        }*/
    }

    public async void TriggerCreatePlayAsync(string name)
    {
        var burner = await burnerManager.DeployBurner(new SigningKey());
        var nameHex = StringToHexString(name);
        var txHash = await accountSystem.Create(burner, dojoConfig.worldAddress, nameHex);
        // Do something with txHash, like logging it
        Debug.Log($"Transaction Hash: {txHash.Hex()}");
    }
}