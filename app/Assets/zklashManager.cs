using Dojo;
using UnityEngine;

public class zklashManager : MonoBehaviour
{
    public static zklashManager instance;
    [SerializeField]
    private GameObject _worldManagerPrefab;
    [SerializeField]
    private GameObject _unityMainThreadDispatcherPrefab;

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
            Debug.LogError("Prefab is not assigned in PrefabManager.");
        }
        if (_unityMainThreadDispatcherPrefab != null)
        {
            Instantiate(_unityMainThreadDispatcherPrefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Prefab is not assigned in PrefabManager.");
        }
    }
}
