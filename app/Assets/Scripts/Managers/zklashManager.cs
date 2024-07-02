using Dojo;
using UnityEngine;

public class zklashManager : MonoBehaviour
{
    public static zklashManager instance;

    [SerializeField] private GameObject _worldManagerPrefab;
    [SerializeField] private GameObject _unityMainThreadDispatcherPrefab;
    [SerializeField] private GameObject _starknetJsInteropPrefab;
    [SerializeField] private GameObject _canvasWaitForTransactionPrefab;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (_worldManagerPrefab != null)
        {
            Instantiate(_worldManagerPrefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogError("worldManagerPrefab is not assigned in PrefabManager.");
        }
        if (_unityMainThreadDispatcherPrefab != null)
        {
            Instantiate(_unityMainThreadDispatcherPrefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogError("unityMainThreadDispatcherPrefab is not assigned in PrefabManager.");
        }

        if (_starknetJsInteropPrefab != null)
        {
            Instantiate(_starknetJsInteropPrefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogError("starknetJsInteropPrefab is not assigned in PrefabManager.");
        }

        if (_canvasWaitForTransactionPrefab != null)
        {
            Instantiate(_canvasWaitForTransactionPrefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogError("canvasWaitForTransactionPrefab is not assigned in PrefabManager.");
        }
    }
}
