using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManagerTest : MonoBehaviour
{
    public List<GameObject> allyPrefabs = new List<GameObject>();
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    public List<GameObject> allySpots = new List<GameObject>();
    public List<GameObject> enemySpots = new List<GameObject>();

    private List<GameObject> allies = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        // Spawn allyPrefabs at their spots
        for (int i = 0; i < Mathf.Min(allyPrefabs.Count, allySpots.Count); i++)
        {
            if (allyPrefabs[i] != null && allySpots[i] != null)
            {
                allies.Add(Instantiate(allyPrefabs[i], allySpots[i].transform.position, Quaternion.identity));
            }
        }

        // Spawn enemyPrefabs at their spots
        for (int i = 0; i < Mathf.Min(enemyPrefabs.Count, enemySpots.Count); i++)
        {
            if (enemyPrefabs[i] != null && enemySpots[i] != null)
            {
                enemies.Add(Instantiate(enemyPrefabs[i], enemySpots[i].transform.position, Quaternion.identity));
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(BattleRoutine());
        }
    }

    IEnumerator BattleRoutine()
    {
        while (allies.Count > 0 && enemies.Count > 0)
        {
            yield return StartCoroutine(PerformRound());
            yield return StartCoroutine(RepositionTeams());
            yield return new WaitForSeconds(2f);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        Debug.Log("Battle Ended");
    }

    IEnumerator Attack(GameObject attacker, GameObject defender)
    {
        MobAttack attackerAttack = attacker.GetComponent<MobAttack>();
        MobHealth defenderHealth = defender.GetComponent<MobHealth>();

        Debug.Log("-------->" + attackerAttack.mobData.name + " is attacking " + defenderHealth.mobData.name);

        if (attackerAttack != null || defenderHealth != null)
        {
            // Set the target of the MobAttack to the MobHealth component of the enemy
            attackerAttack.target = defender.GetComponent<MobHealth>();
            // Set the source of the MobHealth to the MobAttack component of the attacker
            defenderHealth.source = attacker.GetComponent<MobHealth>();
            yield return StartCoroutine(attackerAttack.TriggerAttackCoroutine());
        }
        else
        {
            Debug.Log("MobAttack component not found on attacker or MobHealth component not found on defender.");
        }
    }

    IEnumerator PerformRound()
    {
        Debug.Log("Performing round");
        Coroutine attack1 = StartCoroutine(Attack(allies[0], enemies[0]));
        Coroutine attack2 = StartCoroutine(Attack(enemies[0], allies[0]));
        yield return attack1;
        yield return attack2;

        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator RepositionTeams()
    {
        Debug.Log("Repositioning teams");
        if (IsFirstMobDead(allies))
        {
            for (int i = 1; i < allies.Count; i++)
            {
                GameObject ally = allies[i]; // Access the ally at index i
                MobMovement mobMovement = ally.GetComponent<MobMovement>();
                if (mobMovement != null)
                {
                    mobMovement.Move(allySpots[i - 1].transform);
                }
            }

            // Remove the first mob and update the list, shifting all others down by one
            allies.RemoveAt(0);
        }

        if (IsFirstMobDead(enemies))
        {
            for (int i = 1; i < enemies.Count; i++)
            {
                GameObject ally = enemies[i]; // Access the ally at index i
                MobMovement mobMovement = ally.GetComponent<MobMovement>();
                if (mobMovement != null)
                {
                    mobMovement.Move(enemySpots[i - 1].transform);
                }
            }

            // Remove the first mob and update the list, shifting all others down by one
            enemies.RemoveAt(0);
        }

        yield return null;
    }

    bool IsFirstMobDead(List<GameObject> mobs)
    {
        if (mobs != null && mobs.Count > 0)
        {
            MobHealth mobHealth = mobs[0].GetComponent<MobHealth>();
            Debug.Log(mobHealth.health);
            return mobHealth != null && mobHealth.health <= 0;
        }
        return false;
    }
}
