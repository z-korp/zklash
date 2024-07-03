using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;

public class StarknetJsInterop : MonoBehaviour
{
    public static StarknetJsInterop Instance { get; private set; }

    [DllImport("__Internal")]
    private static extern void WaitForTransaction(string txHash, string options, Action<int, IntPtr> callback);

    private static Dictionary<string, TaskCompletionSource<TransactionResult>> taskCompletionSources = new Dictionary<string, TaskCompletionSource<TransactionResult>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public Task<TransactionResult> WaitForTransactionWrapper(string txHash, string optionsJson)
    {
        var tcs = new TaskCompletionSource<TransactionResult>();
        taskCompletionSources[txHash] = tcs;

        WaitForTransaction(txHash, optionsJson, StaticCallback);
        return tcs.Task;
    }

    [AOT.MonoPInvokeCallback(typeof(Action<int, IntPtr>))]
    private static void StaticCallback(int success, IntPtr resultPtr)
    {
        string resultJson = Marshal.PtrToStringAnsi(resultPtr);
        var result = JsonUtility.FromJson<TransactionResult>(resultJson);

        if (taskCompletionSources.TryGetValue(result.txHash, out var tcs))
        {
            taskCompletionSources.Remove(result.txHash);
            if (success == 1)
            {
                tcs.SetResult(result);
            }
            else
            {
                tcs.SetException(new Exception(resultJson));
            }
        }
        else
        {
            Debug.LogError("No matching TaskCompletionSource found for txHash: " + result.txHash);
        }
    }

    [Serializable]
    public class TransactionResult
    {
        public string txHash;
        public string executionStatus;
        public string finalityStatus;
        public string receipt;
    }
}