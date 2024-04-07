using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public List<GameObject> allies;
    public List<GameObject> enemies;

    public bool isCoroutineRunning = false;
    public float delay = 0.1f;

    void Start()
    {
        StartCoroutine(FigthRoutine());
    }
    void Update()
    {

    }

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


    IEnumerator FigthRoutine()
    {
        yield return new WaitForSeconds(delay);
        DealDamageEnemy(1, 2);
        yield return new WaitForSeconds(delay);
        ReceiveDamageAlly(1, 2);
        yield return new WaitForSeconds(delay);
        DealDamageAlly(1, 2);
        yield return new WaitForSeconds(delay);
        ReceiveDamageEnemy(1, 2);
        yield return new WaitForSeconds(delay);
        DealDamageEnemy(1, 2);
        yield return new WaitForSeconds(delay);
        ReceiveDamageAlly(1, 2);
    }


}
