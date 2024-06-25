using UnityEngine;

public class zklashManager : MonoBehaviour
{
    public static zklashManager instance;
    [SerializeField]
    private GameObject _worldManagerPrefab;

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
    }
}
