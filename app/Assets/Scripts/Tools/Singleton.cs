using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("No instance of " + typeof(T) + " exists in the scene.");

            return _instance;
        }
    }

    protected void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            Init();
        }
        else
        {
            Debug.LogError("An instance of " + typeof(T) + " already exists in the scene. Self-destructing.");
            Destroy(gameObject);
        }
    }

    protected void OnDestroy()
    {
        if (this == _instance)
            _instance = null;

    }

    protected virtual void Init() { }

}
