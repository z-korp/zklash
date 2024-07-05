using System;
using System.Threading.Tasks;
using Dojo;
using Dojo.Starknet;
using UnityEngine;
using dojo_bindings;
using System.Collections;

public class TxCoroutines : MonoBehaviour
{
    public static TxCoroutines Instance { get; private set; }

    [SerializeField] AccountSystem accountSystem;
    [SerializeField] MarketSystem marketSystem;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] WorldManagerData dojoConfig;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            //Debug.LogError("Multiple instances of TxCoroutines found");
        }
    }

    // For tasks that return a value
    private IEnumerator AwaitTask<T>(Task<T> task, Action<T> continuation)
    {
        while (!task.IsCompleted)
        {
            yield return new WaitForSeconds(0.2f); // every 0.2 seconds
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
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Task Failed: " + task.Exception.Flatten());
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

    private IEnumerator ExecuteTransaction(Func<Task<FieldElement>> transactionFunc, Action onSuccess = null, Action<string> onError = null)
    {
        string txHash = "";
        CanvasWaitForTransaction.Instance.ToggleCanvas(true);
        yield return StartCoroutine(AwaitTask(transactionFunc(), (result) => txHash = result.Hex()));

#if UNITY_WEBGL && !UNITY_EDITOR
        if (!string.IsNullOrEmpty(txHash))
        {
            CanvasWaitForTransaction.Instance.setTxHash(txHash);
            string optionsJson = "{ \"nodeUrl\": \"" + dojoConfig.rpcUrl + "\", \"retryInterval\": 100 }";

            Task<StarknetJsInterop.TransactionResult> waitTask = StarknetJsInterop.Instance.WaitForTransactionWrapper(txHash, optionsJson);
            yield return new WaitUntil(() => waitTask.IsCompleted);

            if (waitTask.IsCompletedSuccessfully)
            {
                StarknetJsInterop.TransactionResult result = waitTask.Result;
                Debug.Log("Transaction Status: " + result.executionStatus);
                if (result.executionStatus == "SUCCEEDED")
                {
                    onSuccess?.Invoke();
                }
                else
                {
                    string errorMessage = "Transaction failed with status: " + result.executionStatus;
                    Debug.LogError(errorMessage);
                    onError?.Invoke(errorMessage);
                }
            }
            else if (waitTask.IsFaulted)
            {
                string error = waitTask.Exception?.InnerException?.Message ?? "Unknown error";
                Debug.LogError("Error waiting for transaction: " + error);
                onError?.Invoke(error);
            }
        }
        else
        {
            string error = "Create transaction hash is empty or null.";
            Debug.LogError(error);
            onError?.Invoke(error);
        }
#else
        yield return new WaitForSeconds(1.0f);
        onSuccess?.Invoke();
#endif
        CanvasWaitForTransaction.Instance.setTxHash("");
        CanvasWaitForTransaction.Instance.ToggleCanvas(false);
    }

    // Account System
    public IEnumerator ExecuteCreateAndSpawn(string name)
    {
        Debug.Log("[[[[[[[[[[[  ExecuteCreateAndSpawn  ]]]]]]]]]]]");

        yield return StartCoroutine(ExecuteCreate(name));
        yield return StartCoroutine(ExecuteSpawn());

        Debug.Log("[[[[[[[[[[[ END ExecuteCreateAndSpawn ]]]]]]]]]]]");
    }

    public IEnumerator ExecuteCreate(string name)
    {
        Account account = GameManager.Instance.burnerManager.CurrentBurner;
        string world = GameManager.Instance.dojoConfig.worldAddress;
        var nameHex = StringToHexString(name);

        yield return StartCoroutine(ExecuteTransaction(
            () => accountSystem.Create(account, world, nameHex),
            onSuccess: () => Debug.Log("Create transaction was successful."),
            onError: (error) => Debug.LogError("Create transaction failed: " + error)
        ));
    }

    public IEnumerator ExecuteSpawn()
    {
        Account account = GameManager.Instance.burnerManager.CurrentBurner;
        string world = GameManager.Instance.dojoConfig.worldAddress;

        yield return StartCoroutine(ExecuteTransaction(
            () => accountSystem.Spawn(account, world),
            onSuccess: () => Debug.Log("Spawn transaction was successful."),
            onError: (error) => Debug.LogError("Spawn transaction failed: " + error)
        ));
    }

    // Market System
    public IEnumerator ExecuteEquip(uint team_id, byte character_id, uint index, Action onSuccess = null, Action<string> onError = null)
    {
        Debug.Log("[[[[[[[[[[[  ExecuteEquip  ]]]]]]]]]]] team_id:" + team_id + " character_id:" + character_id + " index:" + index);
        Account account = GameManager.Instance.burnerManager.CurrentBurner;
        string world = GameManager.Instance.dojoConfig.worldAddress;

        yield return StartCoroutine(ExecuteTransaction(
            () => marketSystem.Equip(account, world, team_id, character_id, index),
            onSuccess: () => onSuccess?.Invoke(),
            onError: (error) => onError?.Invoke(error)
        ));

        Debug.Log("[[[[[[[[[[[ END ExecuteEquip ]]]]]]]]]]]");
    }

    public IEnumerator ExecuteHire(uint team_id, uint index, Action onSuccess = null, Action<string> onError = null)
    {
        Debug.Log("[[[[[[[[[[[  ExecuteHire  ]]]]]]]]]]] team_id:" + team_id + " index:" + index);
        Account account = GameManager.Instance.burnerManager.CurrentBurner;
        string world = GameManager.Instance.dojoConfig.worldAddress;

        yield return StartCoroutine(ExecuteTransaction(
            () => marketSystem.Hire(account, world, team_id, index),
            onSuccess: () => onSuccess?.Invoke(),
            onError: (error) => onError?.Invoke(error)
        ));

        Debug.Log("[[[[[[[[[[[ END ExecuteHire ]]]]]]]]]]]");
    }

    public IEnumerator ExecuteReroll(uint team_id, Action onFinish = null)
    {
        Debug.Log("[[[[[[[[[[[  ExecuteReroll  ]]]]]]]]]]] team_id:" + team_id);
        Account account = GameManager.Instance.burnerManager.CurrentBurner;
        string world = GameManager.Instance.dojoConfig.worldAddress;

        yield return StartCoroutine(ExecuteTransaction(
            () => marketSystem.Reroll(account, world, team_id),
            onSuccess: () => Debug.Log("Reroll transaction was successful."),
            onError: (error) => Debug.LogError("Reroll transaction failed: " + error)
        ));

        yield return new WaitForSeconds(1f); // to be sure Torii has indexed (TODO: remove this when Torii is fixed)

        onFinish?.Invoke();  // Call the callback if it's provided

        Debug.Log("[[[[[[[[[[[ END ExecuteReroll ]]]]]]]]]]]");
    }

    public IEnumerator ExecuteMerge(uint team_id, uint from_id, uint to_id, Action onSuccess = null, Action<string> onError = null)
    {
        Debug.Log("[[[[[[[[[[[  ExecuteMerge  ]]]]]]]]]]] team_id:" + team_id + " from_id:" + from_id + " to_id:" + to_id);
        Account account = GameManager.Instance.burnerManager.CurrentBurner;
        string world = GameManager.Instance.dojoConfig.worldAddress;

        yield return StartCoroutine(ExecuteTransaction(
            () => marketSystem.Merge(account, world, team_id, from_id, to_id),
            onSuccess: () => onSuccess?.Invoke(),
            onError: (error) => onError?.Invoke(error)
        ));

        Debug.Log("[[[[[[[[[[[ END ExecuteMerge ]]]]]]]]]]]");
    }

    public IEnumerator ExecuteMergeFromShop(uint team_id, uint character_id, uint index, Action onSuccess = null, Action<string> onError = null)
    {
        Debug.Log("[[[[[[[[[[[  ExecuteMergeFromShop  ]]]]]]]]]]] team_id:" + team_id + " character_id:" + character_id + " index:" + index);
        Account account = GameManager.Instance.burnerManager.CurrentBurner;
        string world = GameManager.Instance.dojoConfig.worldAddress;

        yield return StartCoroutine(ExecuteTransaction(
            () => marketSystem.MergeFromShop(account, world, team_id, character_id, index),
            onSuccess: () => onSuccess?.Invoke(),
            onError: (error) => onError?.Invoke(error)
        ));

        Debug.Log("[[[[[[[[[[[ END ExecuteMergeFromShop ]]]]]]]]]]]");
    }

    public IEnumerator ExecuteSell(uint team_id, uint character_id, Action onSuccess = null, Action<string> onError = null)
    {
        Debug.Log("[[[[[[[[[[[  ExecuteSell  ]]]]]]]]]]]");
        Account account = GameManager.Instance.burnerManager.CurrentBurner;
        string world = GameManager.Instance.dojoConfig.worldAddress; yield return StartCoroutine(ExecuteTransaction(
        () => marketSystem.Sell(account, world, team_id, character_id),
        onSuccess: () => onSuccess?.Invoke(),
        onError: (error) => onError?.Invoke(error)
    ));

        Debug.Log("[[[[[[[[[[[ END ExecuteSell ]]]]]]]]]]]");
    }

    // Battle System
    public IEnumerator ExecuteStartBattle(uint team_id, uint order)
    {
        Debug.Log("[[[[[[[[[[[  ExecuteStartBattle  ]]]]]]]]]]]");
        Account account = GameManager.Instance.burnerManager.CurrentBurner;
        string world = GameManager.Instance.dojoConfig.worldAddress;

        yield return StartCoroutine(ExecuteTransaction(
            () => battleSystem.StartBattle(account, world, team_id, order),
            onSuccess: () => Debug.Log("Start battle transaction was successful."),
            onError: (error) => Debug.LogError("Start battle transaction failed: " + error)
        ));

        yield return new WaitForSeconds(1f); // to be sure Torii has indexed (TODO: remove this when Torii is fixed)
        Debug.Log("[[[[[[[[[[[ END ExecuteStartBattle ]]]]]]]]]]]");
    }
}
