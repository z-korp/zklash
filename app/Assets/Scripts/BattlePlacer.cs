using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattlePlacer : MonoBehaviour
{
    public GameObject[] unitPrefabs;
    public GameObject[] alliesPlacer;
    public GameObject[] enemiesPlacer;

    private bool hasPlacedMobs = false;

    void Update()
    {
        if (VillageData.Instance.fighterEventDetails.Count != 0 && !hasPlacedMobs && BattleManager.Instance != null)
        {
            VillageData.Instance.fighterEventDetails.Sort((x, y) => x.Index.CompareTo(y.Index));
            foreach (var fighter in VillageData.Instance.fighterEventDetails)
            {
                Role role = (Role)fighter.Role;
                var name = PrefabMappings.NameToRoleMap[role];
                var prefab = PrefabUtils.FindPrefabByName(unitPrefabs, name);
                if (fighter.CharacterId > 200)
                {
                    // enemy
                    var placer = enemiesPlacer[fighter.Index];
                    var enemy = Instantiate(prefab, placer.transform.position, Quaternion.identity);
                    BattleManager.Instance.AddEnemy(enemy);
                    BattleManager.Instance.characterIdBindings.Add(fighter.CharacterId, fighter.Index);
                    if (enemy.GetComponent<ElementData>() != null)
                    {
                        enemy.GetComponent<ElementData>().currentHealth = (int)fighter.Health;
                        enemy.GetComponent<ElementData>().currentDamage = (int)fighter.Attack;
                    }
                }
                else
                {
                    // ally
                    var placer = alliesPlacer[fighter.Index];
                    var ally = Instantiate(prefab, placer.transform.position, Quaternion.identity);
                    BattleManager.Instance.AddAlly(ally);
                    BattleManager.Instance.characterIdBindings.Add(fighter.CharacterId, fighter.Index);
                    if (ally.GetComponent<ElementData>() != null)
                    {
                        ally.GetComponent<ElementData>().currentHealth = (int)fighter.Health;
                        ally.GetComponent<ElementData>().currentDamage = (int)fighter.Attack;
                    }
                }
            }

            hasPlacedMobs = true; // Ensure this block only runs once
        }
    }
}
