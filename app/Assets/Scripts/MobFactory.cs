using UnityEngine;

public class MobFactory : MonoBehaviour
{
    public GameObject[] prefabs;
    public static MobFactory instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of MobFactory found!");
            return;
        }
        instance = this;
    }

    public GameObject Create(Role role, Transform transform)
    {
        string title = PrefabMappings.NameToRoleMap[role];
        GameObject prefab = PrefabUtils.FindPrefabByName(prefabs, title);
        return Instantiate(prefab, transform.position, Quaternion.identity);
    }
}