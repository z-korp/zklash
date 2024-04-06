using UnityEngine;

public static class PrefabUtils {
    public static GameObject FindPrefabByName(GameObject[] prefabs, string prefabName) {
        foreach (var prefab in prefabs) {
            if (prefab.name == prefabName) {
                return prefab;
            }
        }
        return null; // Return null if no prefab with the given name is found
    }
}
