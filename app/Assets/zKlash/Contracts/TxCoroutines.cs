using System;
using System.Threading.Tasks;
using Dojo;
using Dojo.Starknet;
using UnityEngine;
using dojo_bindings;
using System.Threading.Tasks;
using System.Collections;

public class TxCoroutines : MonoBehaviour
{
    public static TxCoroutines Instance { get; private set; }

    [SerializeField] AccountSystem accountSystem;
    [SerializeField] MarketSystem marketSystem;
    [SerializeField] BattleSystem battleSystem;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of TxCoroutines found");
        }
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

    public async Task AwaitTransaction(string txHash)
    {
        await GameManager.Instance.provider.WaitForTransaction(new FieldElement(txHash));
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

    // ------------------------------------------------
    // Tx from all systems

    // Account System
    public IEnumerator ExecuteCreateAndSpawn(string name)
    {
        Debug.Log("[[[[[[[[[[[  ExecuteCreateAndSpawn  ]]]]]]]]]]]");
        Account account = GameManager.Instance.burnerManager.CurrentBurner;
        string world = GameManager.Instance.dojoConfig.worldAddress;
        var nameHex = StringToHexString(name);

        string createTxHash = "";
        yield return StartCoroutine(AwaitTask(accountSystem.Create(account, world, nameHex), (result) => createTxHash = result.Hex()));
        yield return StartCoroutine(AwaitTask(AwaitTransaction(createTxHash))); ;

        string spawnTxHash = "";
        yield return StartCoroutine(AwaitTask(accountSystem.Spawn(account, world), (result) => spawnTxHash = result.Hex()));
        yield return StartCoroutine(AwaitTask(AwaitTransaction(spawnTxHash)));
    }

    // Market System
    public IEnumerator ExecuteEquip(uint team_id, byte character_id, uint index)
    {
        Debug.Log("[[[[[[[[[[[  ExecuteEquip  ]]]]]]]]]]]");
        Account account = GameManager.Instance.burnerManager.CurrentBurner;
        string world = GameManager.Instance.dojoConfig.worldAddress;

        string txHash = "";
        yield return StartCoroutine(
            AwaitTask(
                marketSystem.Equip(account, world, team_id, character_id, index),
                (result) => txHash = result.Hex()
            )
        );
        yield return StartCoroutine(AwaitTask(AwaitTransaction(txHash)));
    }

    public IEnumerator ExecuteHire(uint team_id, uint index)
    {
        Debug.Log("[[[[[[[[[[[  ExecuteHire  ]]]]]]]]]]]");
        Account account = GameManager.Instance.burnerManager.CurrentBurner;
        string world = GameManager.Instance.dojoConfig.worldAddress;

        string txHash = "";
        yield return StartCoroutine(
            AwaitTask(
                marketSystem.Hire(account, world, team_id, index),
                (result) => txHash = result.Hex()
            )
        );
        yield return StartCoroutine(AwaitTask(AwaitTransaction(txHash)));
    }

    public IEnumerator ExecuteReroll(uint team_id)
    {
        Debug.Log("[[[[[[[[[[[  ExecuteReroll  ]]]]]]]]]]]");
        Account account = GameManager.Instance.burnerManager.CurrentBurner;
        string world = GameManager.Instance.dojoConfig.worldAddress;

        string txHash = "";
        yield return StartCoroutine(
            AwaitTask(
                marketSystem.Reroll(account, world, team_id),
                (result) => txHash = result.Hex()
            )
        );
        yield return StartCoroutine(AwaitTask(AwaitTransaction(txHash)));
    }

    public IEnumerator ExecuteMerge(uint team_id, uint from_id, uint to_id)
    {
        Debug.Log("[[[[[[[[[[[  ExecuteMerge  ]]]]]]]]]]]");
        Account account = GameManager.Instance.burnerManager.CurrentBurner;
        string world = GameManager.Instance.dojoConfig.worldAddress;

        string txHash = "";
        yield return StartCoroutine(
            AwaitTask(
                marketSystem.Merge(account, world, team_id, from_id, to_id),
                (result) => txHash = result.Hex()
            )
        );
        yield return StartCoroutine(AwaitTask(AwaitTransaction(txHash)));
    }

    public IEnumerator ExecuteMergeFromShop(uint team_id, uint character_id, uint index)
    {
        Debug.Log("[[[[[[[[[[[  ExecuteMergeFromShop  ]]]]]]]]]]]");
        Account account = GameManager.Instance.burnerManager.CurrentBurner;
        string world = GameManager.Instance.dojoConfig.worldAddress;

        string txHash = "";
        yield return StartCoroutine(
            AwaitTask(
                marketSystem.MergeFromShop(account, world, team_id, character_id, index),
                (result) => txHash = result.Hex()
            )
        );
        yield return StartCoroutine(AwaitTask(AwaitTransaction(txHash)));
    }

    public IEnumerator ExecuteSell(uint team_id, uint character_id)
    {
        Debug.Log("[[[[[[[[[[[  ExecuteSell  ]]]]]]]]]]]");
        Account account = GameManager.Instance.burnerManager.CurrentBurner;
        string world = GameManager.Instance.dojoConfig.worldAddress;

        string txHash = "";
        yield return StartCoroutine(
            AwaitTask(
                marketSystem.Sell(account, world, team_id, character_id),
                (result) => txHash = result.Hex()
            )
        );
        yield return StartCoroutine(AwaitTask(AwaitTransaction(txHash)));
    }

    // Battle System
    public IEnumerator ExecuteStartBattle(uint team_id, uint order)
    {
        Debug.Log("[[[[[[[[[[[  ExecuteStartBattle  ]]]]]]]]]]]");
        Account account = GameManager.Instance.burnerManager.CurrentBurner;
        string world = GameManager.Instance.dojoConfig.worldAddress;

        string txHash = "";
        yield return StartCoroutine(
            AwaitTask(
                battleSystem.StartBattle(account, world, team_id, order),
                (result) => txHash = result.Hex()
            )
        );
        yield return StartCoroutine(AwaitTask(AwaitTransaction(txHash)));
    }
}