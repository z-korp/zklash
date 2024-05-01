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
using zKlash.Game.Roles;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public WorldManager worldManager;

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
                TeamManager.instance.UpdateMissingEntity(entity.name);
            }
        }

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

    public IEnumerator TriggerCreateAndSpawn(string name)
    {
        Debug.Log("TriggerCreateAndSpawn");
        Account currentBurner = burnerManager.CurrentBurner;
        var nameHex = StringToHexString(name);

        // Call the async method and wait for it to complete
        string createTxHash = "";
        yield return StartCoroutine(AwaitTask(CreateTransaction(currentBurner, nameHex), (result) => createTxHash = result));
        yield return StartCoroutine(AwaitTask(AwaitTransaction(createTxHash))); ;

        // Repeat for the spawning process
        string spawnTxHash = "";
        yield return StartCoroutine(AwaitTask(SpawnTransaction(currentBurner), (result) => spawnTxHash = result));
        yield return StartCoroutine(AwaitTask(AwaitTransaction(spawnTxHash)));
    }

    // For tasks that return a value
    private IEnumerator AwaitTask<T>(Task<T> task, Action<T> continuation)
    {
        while (!task.IsCompleted)
        {
            yield return null;
        }

        if (task.IsFaulted)
        {
            Debug.LogError("Task Failed: " + task.Exception.Flatten());
        }
        else if (task.IsCompleted)
        {
            try
            {
                continuation(task.Result);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error within continuation: " + ex);
            }
        }
    }

    // For tasks that do not return a value
    private IEnumerator AwaitTask(Task task)
    {
        while (!task.IsCompleted)
            yield return null;

        if (task.IsFaulted)
        {
            Debug.LogError("Task Failed: " + task.Exception.Flatten());
        }
    }

    public async Task<string> CreateTransaction(Account currentBurner, string nameHex)
    {
        try
        {
            var txHash = await accountSystem.Create(currentBurner, dojoConfig.worldAddress, nameHex);
            return txHash.Hex();
        }
        catch (Exception ex)
        {
            Debug.LogError($"CreateTransaction failed: {ex.Message}");
            return null;
        }
    }

    public async Task<string> SpawnTransaction(Account currentBurner)
    {
        try
        {
            var txHash = await accountSystem.Spawn(currentBurner, dojoConfig.worldAddress);
            return txHash.Hex();
        }
        catch (Exception ex)
        {
            Debug.LogError($"CreateTransaction failed: {ex.Message}");
            return null;
        }
    }

    public async Task AwaitTransaction(string txHash)
    {
        await provider.WaitForTransaction(new FieldElement(txHash));
    }
}