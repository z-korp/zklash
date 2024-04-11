using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;


public class BattleManager : MonoBehaviour
{
    public static BattleManager instance { get; private set; }
    public List<GameObject> allies = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>(); 
    public float delay = 1f;
    public Dictionary<uint, uint> characterIdBindings = new Dictionary<uint, uint>();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of BattleManager found.");
            return;
        }

        instance = this;
    }

    void Update()
    {
        // if (allies.Count > 0 && enemies.Count > 0 && characterIdBindings.Count > 0 && !isCoroutineRunning)
        // {
            // isCoroutineRunning = true;
            // StartCoroutine(StartBattle(() =>
            //         {
            //             // Code to execute after the coroutine finishes
            //             SceneManager.LoadScene("Shop");
            //         }));
        // }
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


    IEnumerator StartBattle(Action onCompleted = null)
    {
        // foreach (var detail in combinedEventDetails)
        // {
        //     if (detail is Hit)
        //     {
        //         Hit hit = (Hit)detail;
        //         int fromIndex = (int)characterIdBindings[hit.FromCharacterId];
        //         int toIndex = (int)characterIdBindings[hit.ToCharacterId];


        //         if (hit.Damage > 0)
        //         {
        //             if (hit.FromCharacterId > 200) // enemy
        //             {
        //                 Debug.Log($"Event: {hit.GetType().Name}, [ENEMY] Tick: {hit.Tick}, From: {fromIndex}, To: {toIndex}, Damage: {hit.Damage}");
        //                 yield return StartCoroutine(EnemyHit(fromIndex, toIndex, (int)hit.Damage));
        //             }
        //             else // ally
        //             {
        //                 Debug.Log($"Event: {hit.GetType().Name}, [ALLY] Tick: {hit.Tick}, From: {fromIndex}, To: {toIndex}, Damage: {hit.Damage}");
        //                 yield return StartCoroutine(AllyHit(fromIndex, toIndex, (int)hit.Damage));
        //             }
        //         }
        //     }
        //     else if (detail is Usage)
        //     {
        //         Usage usage = (Usage)detail;
        //         //Debug.Log($"Event: {usage.GetType().Name}, Tick: {usage.Tick}, CharacterId: {usage.CharacterId}, ItemId: {usage.ItemId}");

        //     }
        //     else if (detail is Talent)
        //     {
        //         Talent talent = (Talent)detail;
        //         //Debug.Log($"Event: {talent.GetType().Name}, Tick: {talent.Tick}, CharacterId: {talent.CharacterId}, TalentId: {talent.TalentId}");

        //     }
        // }
        // onCompleted?.Invoke();
        yield return null;
    }

    IEnumerator AllyHit(int indexAlly, int indexEnemy, int dmg)
    {
        DealDamageAlly(indexAlly, dmg);
        yield return new WaitForSeconds(delay / 10);
        bool isDead = ReceiveDamageEnemy(indexEnemy, dmg);
        if (isDead)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                yield return new WaitForSeconds(delay);
                // Move each enemy to the new position based on the updated index
                MoveEnemy(i); // Assumes MoveEnemy moves the enemy based on its index in the list
            }
        }
        yield return new WaitForSeconds(delay);
    }

    IEnumerator EnemyHit(int indexEnemy, int indexAlly, int dmg)
    {
        DealDamageEnemy(indexEnemy, dmg);
        yield return new WaitForSeconds(delay / 10);
        bool isDead = ReceiveDamageAlly(indexAlly, dmg);
        if (isDead)
        {
            for (int i = 0; i < allies.Count; i++)
            {
                yield return new WaitForSeconds(delay);
                // Move each ally to the new position based on the updated index
                MoveAlly(i); // Assumes MoveAlly moves the ally based on its index in the list
            }
        }
        yield return new WaitForSeconds(delay);
    }

    // Ally methodes
    public bool ReceiveDamageAlly(int indexAlly, int amount)
    {
        ElementData elementData = allies.ElementAt(indexAlly)?.GetComponent<ElementData>();
        if (elementData != null)
        {
            return elementData.TakeDamage(amount);
        }
        else
        {
            Debug.LogError("ElementData is null");
        }
        return false;
    }

    public void DealDamageAlly(int indexAlly, int amount)
    {
        ElementData elementData = allies.ElementAt(indexAlly)?.GetComponent<ElementData>();
        if (elementData != null)
        {
            elementData.DealDamage(amount);
        }
        else
        {
            Debug.LogError("ElementData is null");
        }
    }

    public void MoveAlly(int indexAlly)
    {
        ElementData elementData = allies.ElementAt(indexAlly)?.GetComponent<ElementData>();
        if (elementData != null)
        {
            elementData.MoveAlly();
        }
        else
        {
            Debug.LogError("ElementData is null");
        }
    }
    public void HealUpAlly(int indexAlly, int amount)
    {
        ElementData elementData = allies.ElementAt(indexAlly)?.GetComponent<ElementData>();
        if (elementData != null)
        {
            elementData.HealPlayer(amount);
        }
        else
        {
            Debug.LogError("ElementData is null");
        }
    }

    public void PowerUpAlly(int indexAlly, int amount)
    {
        ElementData elementData = allies.ElementAt(indexAlly)?.GetComponent<ElementData>();
        if (elementData != null)
        {
            elementData.PowerUp(amount);
        }
        else
        {
            Debug.LogError("ElementData is null");
        }
    }

    // Enemy methodes

    public bool ReceiveDamageEnemy(int indexEnemy, int amount)
    {
        ElementData elementData = enemies.ElementAt(indexEnemy)?.GetComponent<ElementData>();
        if (elementData != null)
        {
            return elementData.TakeDamage(amount); ;
        }
        else
        {
            Debug.LogError("ElementData is null");
        }
        return false;
    }

    public void DealDamageEnemy(int indexEnemy, int amount)
    {
        ElementData elementData = enemies.ElementAt(indexEnemy)?.GetComponent<ElementData>();
        if (elementData != null)
        {
            elementData.DealDamage(amount);
        }
        else
        {
            Debug.LogError("ElementData is null");
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
        /*MoveAlly(1);
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
        ReceiveDamageEnemy(2, 5);*/


    }


}
