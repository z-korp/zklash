using UnityEngine;

public class PersistentManager : MonoBehaviour
{
    private static bool instanceExists = false;

    void Awake()
    {
        if (!instanceExists)
        {
            DontDestroyOnLoad(gameObject);
            instanceExists = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
