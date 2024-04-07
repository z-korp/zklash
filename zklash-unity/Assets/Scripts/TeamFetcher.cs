using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamFetcher : MonoBehaviour
{
    public GameObject[] unitPrefabs; // Préfabs pour les trois premiers éléments

    // Start is called before the first frame update
    void Start()
    {
        foreach (var spot in VillageData.Instance.Spots)
        {
            if (spot.IsAvailable)
            {
                continue;
            }

            int index = VillageData.Instance.Spots.IndexOf(spot);
            var character = GameManager.Instance.worldManager.Entity(spot.EntityContained).GetComponent<Character>();

            string gameObjectName = $"DroppableZone_{index}";
            Debug.Log($"Looking for GameObject: {gameObjectName}");
            GameObject zoneGameObject = GameObject.Find(gameObjectName);
            if (zoneGameObject != null)
            {
                Role role = (Role)character.role;
                var prefabToPlace = PrefabUtils.FindPrefabByName(unitPrefabs, PrefabMappings.NameToRoleMap[role]);
                if (prefabToPlace == null)
                {
                    Debug.LogError($"Prefab not found for role: {name}");
                    return;
                }
                // Instantiate the prefab at the center of the zoneGameObject
                GameObject instance = Instantiate(prefabToPlace, zoneGameObject.transform.position, Quaternion.identity);
                if (instance != null)
                {
                    ElementData data = instance.GetComponent<ElementData>();
                    data.entity = spot.EntityContained;
                }

                Debug.Log($"Instantiated prefab at {zoneGameObject.name}");
            }
            else
            {
                Debug.LogWarning($"GameObject not found for index: {index}");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
