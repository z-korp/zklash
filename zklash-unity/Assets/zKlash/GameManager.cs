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

        //worldManager.synchronizationMaster.OnSynchronized.AddListener(InitEntity2);
    }

    void Update()
    {
        
    }

    private void InitEntity2(List<GameObject> entities)
    {
        Debug.Log($"---------------------------------");
        foreach (var entity in entities)
        {
            
            Debug.Log($"Entity spawned with id: {entity.name}");
            
        }
        Debug.Log($"---------------------------------");
    }

    private void InitEntity(GameObject entity)
    {
        Account currentBurner = burnerManager.CurrentBurner;
        if (currentBurner == null)
        {
            Debug.Log("No current burner");
            return;
        }

        Player playerComponent = entity.GetComponent<Player>();
        if (playerComponent != null)
        {
            // This entity is of type Tile, perform actions with its index

            Debug.Log($"---------------------------------");
            Debug.Log($"currentBurner: {currentBurner.Address.Hex()}");
            Debug.Log($"Player entity spawned with id: {playerComponent.id.Hex()}");
            if(currentBurner.Address.Hex() == playerComponent.id.Hex())
            {
                Debug.Log(">>>>>>>>>>>> Current player information stored.");
            }
            else
            {
                Debug.Log(">>>>>>>>>>>> Current player information not stored.");
            }
            Debug.Log($"---------------------------------");
        }

        Shop shopComponent = entity.GetComponent<Shop>();
        if (shopComponent != null)
        {
            if(shopComponent.player_id === currentBurner.Address.Hex())
            {
                Debug.Log(">>>>>>>>>>>> Current shop information stored.");
            }
            else
            {
                Debug.Log(">>>>>>>>>>>> Current shop information not stored.");
            }
        }
    }

    public async void TriggerCreatePlayAsync(string name)
    {
        var burner = await burnerManager.DeployBurner(new SigningKey());
        var nameHex = StringToHexString(name);
        var txHash = await accountSystem.Create(burner, dojoConfig.worldAddress, nameHex);
        // Do something with txHash, like logging it
        Debug.Log($"[Create] Transaction Hash: {txHash.Hex()}");

        txHash = await accountSystem.Spawn(burner, dojoConfig.worldAddress);
        Debug.Log($"[Spawn] Transaction Hash: {txHash.Hex()}");
    }
}