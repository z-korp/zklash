using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public List<GameObject> allies;
    public List<GameObject> enemies;

    public bool isCoroutineRunning = false;
    public float delay = 0.1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep the manager across scenes
        }
        else
        {
            Destroy(gameObject); // Ensures there's only one instance
        }
    }

    public void AddAlly(GameObject ally)
    {
        if (!allies.Contains(ally))
        {
            allies.Add(ally);
        }
    }

    public void RemoveAlly(GameObject ally)
    {
        if (allies.Contains(ally))
        {
            allies.Remove(ally);
        }
    }

    public void AddEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    void Start()
    {
        // sort fighters by index
        VillageData.Instance.fighterEventDetails.Sort((x, y) => x.Index.CompareTo(y.Index));
        foreach (var fighter in VillageData.Instance.fighterEventDetails)
        {
            Role role = (Role)fighter.Role;
            var prefab = PrefabMappings.NameToRoleMap[role];
            if (fighter.CharacterId > 200)
            {
                // enemy
                //var enemy = Instantiate(prefabToPlace, placePosition, Quaternion.identity);
                //enemies.Add(enemy);
            }
            else
            {
                // ally
                //var ally = Instantiate(prefabToPlace, placePosition, Quaternion.identity);
                //enemies.Add(ally);
            }
        }
        //StartCoroutine(FigthRoutine());
    }

    void Update()
    {

    }

    // Ally methodes
    public void ReceiveDamageAlly(int indexAlly, int amount)
    {
        ElementData elementData = allies.ElementAtOrDefault(indexAlly)?.GetComponent<ElementData>();
        if (elementData != null)
        {
            elementData.TakeDamage(amount);
        }

    }

    public void DealDamageAlly(int indexAlly, int amount)
    {
        ElementData elementData = allies.ElementAtOrDefault(indexAlly)?.GetComponent<ElementData>();
        if (elementData != null)
        {
            elementData.DealDamage(amount);
        }
    }

    public void MoveAlly(int indexAlly)
    {
        ElementData elementData = allies.ElementAtOrDefault(indexAlly)?.GetComponent<ElementData>();
        if (elementData != null)
        {
            elementData.MoveAlly();
        }
    }
    public void HealUpAlly(int indexAlly, int amount)
    {
        ElementData elementData = allies.ElementAtOrDefault(indexAlly)?.GetComponent<ElementData>();
        if (elementData != null)
        {
            elementData.HealPlayer(amount);
        }
    }

    public void PowerUpAlly(int indexAlly, int amount)
    {
        ElementData elementData = allies.ElementAtOrDefault(indexAlly)?.GetComponent<ElementData>();
        if (elementData != null)
        {
            elementData.PowerUp(amount);
        }
    }

    // Enemy methodes

    public void ReceiveDamageEnemy(int indexEnemy, int amount)
    {
        ElementData elementData = enemies.ElementAtOrDefault(indexEnemy)?.GetComponent<ElementData>();
        if (elementData != null)
        {
            elementData.TakeDamage(amount);
        }

    }

    public void DealDamageEnemy(int indexEnemy, int amount)
    {
        ElementData elementData = enemies.ElementAtOrDefault(indexEnemy)?.GetComponent<ElementData>();
        if (elementData != null)
        {
            elementData.DealDamage(amount);
        }
    }

    public void MoveEnemy(int indexEnemy)
    {
        ElementData elementData = enemies.ElementAtOrDefault(indexEnemy)?.GetComponent<ElementData>();
        if (elementData != null)
        {
            elementData.MoveEnemy();
        }
    }


    IEnumerator FigthRoutine()
    {
        yield return new WaitForSeconds(delay);
        DealDamageEnemy(0, 2);
        yield return new WaitForSeconds(delay);
        ReceiveDamageAlly(0, 2);
        yield return new WaitForSeconds(delay);
        DealDamageAlly(0, 2);
        yield return new WaitForSeconds(delay);
        ReceiveDamageEnemy(0, 2);
        yield return new WaitForSeconds(delay);
        DealDamageEnemy(0, 2);
        yield return new WaitForSeconds(delay);
        ReceiveDamageAlly(0, 2);
        yield return new WaitForSeconds(delay);
        MoveAlly(1);
        MoveAlly(2);
        yield return new WaitForSeconds(delay);
        PowerUpAlly(1, 1);
        yield return new WaitForSeconds(delay);
        HealUpAlly(1, 1);
        yield return new WaitForSeconds(delay);
        DealDamageAlly(1, 2);
        yield return new WaitForSeconds(delay);
        ReceiveDamageEnemy(0, 2);
        yield return new WaitForSeconds(delay);
        MoveEnemy(1);
        MoveEnemy(2);
        yield return new WaitForSeconds(delay);
        DealDamageEnemy(1, 10);
        yield return new WaitForSeconds(delay);
        ReceiveDamageAlly(1, 10);
        ReceiveDamageEnemy(1, 10);
        yield return new WaitForSeconds(delay);
        MoveEnemy(2);
        MoveAlly(2);
        yield return new WaitForSeconds(delay);
        PowerUpAlly(2, 3);
        yield return new WaitForSeconds(delay);
        DealDamageAlly(2, 5);
        yield return new WaitForSeconds(delay);
        ReceiveDamageEnemy(2, 5);


    }


}
