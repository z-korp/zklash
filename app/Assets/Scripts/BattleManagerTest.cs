using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zKlash.Game;
using zKlash.Game.Items;
using zKlash.Game.Roles;
using GameCharacter = zKlash.Game.Character;

[System.Serializable] // This attribute makes the struct visible in the Inspector.
public struct CharacterSetup
{
    public Role role;
    public int level;
    public Item item;
}

public class BattleManagerTest : MonoBehaviour
{
    public List<CharacterSetup> alliesSetup;
    public List<CharacterSetup> enemiesSetup;

    public GameObject[] unitPrefabs;

    public List<GameObject> allySpots = new List<GameObject>();
    public List<GameObject> enemySpots = new List<GameObject>();

    private List<GameObject> allies = new List<GameObject>();
    private List<GameCharacter> allyCharacters = new List<GameCharacter>();
    private List<GameObject> enemies = new List<GameObject>();
    private List<GameCharacter> enemyCharacters = new List<GameCharacter>();

    void Start()
    {
        for (int i = 0; i < alliesSetup.Count; i++)
        {
            var ally = alliesSetup[i];

            GameCharacter character = new GameCharacter(ally.role, ally.level, ally.item);
            allyCharacters.Add(character);
            var name = PrefabMappings.NameToRoleMap[ally.role];
            var prefab = PrefabUtils.FindPrefabByName(unitPrefabs, name);

            if (prefab != null && allySpots[i] != null)
            {
                GameObject allyObject = Instantiate(prefab, allySpots[i].transform.position, Quaternion.identity);
                allyObject.GetComponent<MobOrientation>().SetOrientation(MobOrientation.Orientation.Right);
                allies.Add(allyObject);
            }
        }

        for (int i = 0; i < enemiesSetup.Count; i++)
        {
            var enemy = enemiesSetup[i];

            GameCharacter character = new GameCharacter(enemy.role, enemy.level, enemy.item);
            enemyCharacters.Add(character);
            var name = PrefabMappings.NameToRoleMap[enemy.role];
            var prefab = PrefabUtils.FindPrefabByName(unitPrefabs, name);

            if (prefab != null && enemySpots[i] != null)
            {
                GameObject enemyObject = Instantiate(prefab, enemySpots[i].transform.position, Quaternion.identity);
                enemyObject.GetComponent<MobOrientation>().SetOrientation(MobOrientation.Orientation.Left);
                enemies.Add(enemyObject);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(Battle(allies, enemies));
        }
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

    IEnumerator Battle(List<GameObject> team1, List<GameObject> team2)
    {
        if (team1.Count == 0)
        {
            Debug.Log("Team 1 has lost");
            yield break;
        }
        else if (team2.Count == 0)
        {
            Debug.Log("Team 2 has lost");
            yield break;
        }

        if (team1[0].GetComponent<MobHealth>().health <= 0)
        {
            team1.RemoveAt(0);
        }

        if (team2[0].GetComponent<MobHealth>().health <= 0)
        {
            team2.RemoveAt(0);
        }

        GameObject char1 = team1[0];
        applyEffect(char1, Phase.OnDispatch);
        GameObject char2 = team2[0];
        applyEffect(char2, Phase.OnDispatch);

        yield return Duel(char1, char2);

        yield return RepositionTeams();
        yield return new WaitForSeconds(2f);

        yield return Battle(team1, team2);
    }

    IEnumerator Duel(GameObject char1, GameObject char2)
    {
        Debug.Log("============================================================");
        Debug.Log("Dueling");
        Coroutine attack1 = StartCoroutine(Attack(char1, char2));
        Coroutine attack2 = StartCoroutine(Attack(char2, char1));
        yield return attack1;
        yield return attack2;

        yield return new WaitForSeconds(0.1f);

        bool isChar1Dead = char1.GetComponent<MobHealth>().health <= 0;
        bool isChar2Dead = char2.GetComponent<MobHealth>().health <= 0;

        if (isChar1Dead || isChar2Dead)
        {
            Debug.Log("============================================================");
            yield break;
        }

        Debug.Log("============================================================");
        yield return Duel(char1, char2);
    }

    IEnumerator ApplyEffects(GameObject character, Phase phase)
    {
        yield return null;
    }

    (int, int, Buff) applyEffect(GameObject character, Phase phase)
    {
        /*        // [Effect] Apply talent and item buff for char
        let (talent_damage, stun, next_buff) = char.talent(phase, battle_id, tick);
        let item_damage = char.usage(phase, battle_id, tick);
        (talent_damage + item_damage, stun, next_buff)*/
        //Talent talent = character.Talent;
        //Item item = character.GetComponent<MobItem>();



        return (0, 0, new Buff());
    }

    IEnumerator PostMortem()
    {
        yield return null;
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
