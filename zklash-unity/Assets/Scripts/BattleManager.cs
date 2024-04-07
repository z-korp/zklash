using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;


public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public List<GameObject> allies = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();

    public bool isCoroutineRunning = false;
    public float delay = 1f;

    private List<ITickable> combinedEventDetails = new List<ITickable>();

    public Dictionary<uint, uint> characterIdBindings = new Dictionary<uint, uint>();

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

        CombineAndSortEvents();
    }

    void Start()
    {

    }

    void Update()
    {
        if (allies.Count > 0 && enemies.Count > 0 && characterIdBindings.Count > 0 && !isCoroutineRunning)
        {
            isCoroutineRunning = true;
            StartCoroutine(StartBattle(() =>
                    {
                        // Code to execute after the coroutine finishes
                        SceneManager.LoadScene("ShopScene");
                    }));
        }
    }

    private void CombineAndSortEvents()
    {
        // Temporary list to hold all event details before sorting
        List<ITickable> tempCombinedEventDetails = new List<ITickable>();

        // Add event details to the temporary list, assuming they all implement ITickable
        tempCombinedEventDetails.AddRange(VillageData.Instance.hitEventDetails.Cast<ITickable>());
        //tempCombinedEventDetails.AddRange(VillageData.Instance.stunEventDetails.Cast<ITickable>());
        //tempCombinedEventDetails.AddRange(VillageData.Instance.absorbEventDetails.Cast<ITickable>());
        tempCombinedEventDetails.AddRange(VillageData.Instance.usageEventDetails.Cast<ITickable>());
        tempCombinedEventDetails.AddRange(VillageData.Instance.talentEventDetails.Cast<ITickable>());

        // Sort the temporary list based on the Tick property
        var sortedEventDetails = tempCombinedEventDetails.OrderBy(detail => detail.Tick).ToList();

        // Clear the original combinedEventDetails list
        combinedEventDetails.Clear();

        // Repopulate combinedEventDetails with sorted events
        combinedEventDetails.AddRange(sortedEventDetails);


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

    public void AddBindings(uint characterId, uint index)
    {
        characterIdBindings.Add(characterId, index);
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
        foreach (var detail in combinedEventDetails)
        {
            if (detail is Hit)
            {
                Hit hit = (Hit)detail;
                int fromIndex = (int)characterIdBindings[hit.FromCharacterId];
                int toIndex = (int)characterIdBindings[hit.ToCharacterId];


                if (hit.Damage > 0)
                {
                    if (hit.FromCharacterId > 200) // enemy
                    {
                        Debug.Log($"Event: {hit.GetType().Name}, [ENEMY] Tick: {hit.Tick}, From: {fromIndex}, To: {toIndex}, Damage: {hit.Damage}");
                        yield return StartCoroutine(EnemyHit(fromIndex, toIndex, (int)hit.Damage));
                    }
                    else // ally
                    {
                        Debug.Log($"Event: {hit.GetType().Name}, [ALLY] Tick: {hit.Tick}, From: {fromIndex}, To: {toIndex}, Damage: {hit.Damage}");
                        yield return StartCoroutine(AllyHit(fromIndex, toIndex, (int)hit.Damage));
                    }
                }
            }
            else if (detail is Usage)
            {
                Usage usage = (Usage)detail;
                //Debug.Log($"Event: {usage.GetType().Name}, Tick: {usage.Tick}, CharacterId: {usage.CharacterId}, ItemId: {usage.ItemId}");

            }
            else if (detail is Talent)
            {
                Talent talent = (Talent)detail;
                //Debug.Log($"Event: {talent.GetType().Name}, Tick: {talent.Tick}, CharacterId: {talent.CharacterId}, TalentId: {talent.TalentId}");

            }
        }
        onCompleted?.Invoke();
        yield return null;
    }

    IEnumerator AllyHit(int indexAlly, int indexEnemy, int dmg)
    {
        yield return new WaitForSeconds(delay);
        DealDamageAlly(indexAlly, dmg);
        yield return new WaitForSeconds(delay);
        ReceiveDamageEnemy(indexEnemy, dmg);
    }

    IEnumerator EnemyHit(int indexEnemy, int indexAlly, int dmg)
    {
        yield return new WaitForSeconds(delay);
        DealDamageEnemy(indexEnemy, dmg);
        yield return new WaitForSeconds(delay);
        ReceiveDamageAlly(indexAlly, dmg);
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
