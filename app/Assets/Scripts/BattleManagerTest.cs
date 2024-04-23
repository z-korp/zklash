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

    public ItemData[] itemData;

    public List<GameObject> allySpots = new List<GameObject>();
    public List<GameObject> enemySpots = new List<GameObject>();

    private List<GameObject> allies = new List<GameObject>();
    private List<GameCharacter> allyCharacters = new List<GameCharacter>();
    private List<GameObject> enemies = new List<GameObject>();
    private List<GameCharacter> enemyCharacters = new List<GameCharacter>();

    private bool isBattleStarted = false;

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
                allyObject.GetComponent<MobController>().ConfigureCharacter(character);

                Debug.Log("JJJJJJJJJJJJJJJJJJJJJJJJJJJJ Item: " + ally.item);
                var name2 = PrefabMappings.NameToItemDataMap[ally.item];
                Debug.Log("JJJJJJJJJJJJJJJJJJJJJJJJJJJJ Name2: " + name2);
                if (name2 != "None")
                {
                    var item = PrefabUtils.FindScriptableByName(itemData, name2);
                    allyObject.GetComponent<MobItem>().item = item;
                }

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
                enemyObject.GetComponent<MobController>().ConfigureCharacter(character);

                enemies.Add(enemyObject);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isBattleStarted)
        {
            StartCoroutine(Battle(allies, enemies));
            isBattleStarted = true;
        }
    }

    private Buff next_buff1 = new Buff();
    private Buff next_buff2 = new Buff();

    IEnumerator Battle(List<GameObject> team1, List<GameObject> team2)
    {
        if (team1[0].GetComponent<MobHealth>().Health <= 0)
        {
            team1.RemoveAt(0);
            if (team1.Count == 0)
            {
                Debug.Log("Team 1 has lost");
                yield break;
            }

            // [Effect] Apply effects on dispatch
            Debug.Log(">>>>>>>>>> DISPATCH ALLY <<<<<<<<<<<< ");
            applyEffect(team1[0], Phase.OnDispatch);
            // [Effect] Apply floating buff
            team1[0].GetComponent<MobController>().Character.ApplyBuff(next_buff1);
            next_buff1 = new Buff(); // reinit buff for next character
        }

        if (team2[0].GetComponent<MobHealth>().Health <= 0)
        {
            team2.RemoveAt(0);
            if (team2.Count == 0)
            {
                Debug.Log("Team 2 has lost");
                yield break;
            }

            // [Effect] Apply effects on dispatch
            Debug.Log(">>>>>>>>>> DISPATCH ENEMY <<<<<<<<<<<<");
            applyEffect(team2[0], Phase.OnDispatch);
            // [Effect] Apply floating buff
            team2[0].GetComponent<MobController>().Character.ApplyBuff(next_buff2);
            next_buff2 = new Buff(); // reinit buff for next character
        }

        GameObject char1 = team1[0];
        GameObject char2 = team2[0];

        yield return Duel(char1, char2);

        yield return RepositionTeams();
        yield return new WaitForSeconds(2f);

        yield return Battle(team1, team2);
    }

    IEnumerator Duel(GameObject char1, GameObject char2)
    {
        GameCharacter c1 = char1.GetComponent<MobController>().Character;
        GameCharacter c2 = char2.GetComponent<MobController>().Character;

        Debug.Log("[START DUEL]============================================================");
        Debug.Log("Dueling");

        // [Effect] Apply talent and item buff for char1 and char2
        var (damage1, stun1, _) = applyEffect(char1, Phase.OnFight);
        var (damage2, stun2, _) = applyEffect(char2, Phase.OnFight);

        // [Effect] Apply stun effects
        c1.Stun = stun2;
        c2.Stun = stun1;

        Coroutine attack1 = StartCoroutine(Attack(char1, char2, damage1));
        Coroutine attack2 = StartCoroutine(Attack(char2, char1, damage2));
        yield return attack1;
        yield return attack2;

        yield return new WaitForSeconds(0.1f);

        bool isChar1Dead = char1.GetComponent<MobHealth>().Health <= 0;
        bool isChar2Dead = char2.GetComponent<MobHealth>().Health <= 0;

        if (isChar1Dead)
        {
            var (dmg1, buff1) = postMortem(char1, char2);
            if (dmg1 > 0)
                yield return StartCoroutine(PostMortem(char1, char2, dmg1));

            var (dmg2, buff2) = postMortem(char2, char1);
            if (dmg2 > 0)
                yield return StartCoroutine(PostMortem(char2, char1, dmg2));

            next_buff1 = buff1;
            next_buff2 = buff2;
        }
        else if (isChar2Dead)
        {
            var (dmg2, buff2) = postMortem(char2, char1);
            if (dmg2 > 0)
                yield return StartCoroutine(PostMortem(char2, char1, dmg2));

            var (dmg1, buff1) = postMortem(char1, char2);
            if (dmg1 > 0)
                yield return StartCoroutine(PostMortem(char1, char2, dmg1));

            next_buff1 = buff1;
            next_buff2 = buff2;
        }


        Coroutine dead1 = null;
        Coroutine dead2 = null;

        isChar1Dead = char1.GetComponent<MobHealth>().Health <= 0;
        if (isChar1Dead)
        {
            dead1 = StartCoroutine(char1.GetComponent<MobHealth>().TriggerDie());
        }

        isChar2Dead = char2.GetComponent<MobHealth>().Health <= 0;
        if (isChar2Dead)
        {
            dead2 = StartCoroutine(char2.GetComponent<MobHealth>().TriggerDie());
        }

        yield return dead1;
        yield return dead2;

        if (isChar1Dead || isChar2Dead)
        {
            yield return WaitForKey();
            Debug.Log("[END DUEL]============================================================");
            yield break;
        }

        yield return WaitForKey();
        Debug.Log("[END CONTINUE DUEL]============================================================");

        yield return Duel(char1, char2);
    }

    IEnumerator WaitForKey()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
    }

    IEnumerator Attack(GameObject attacker, GameObject defender, int additionalDamage)
    {
        // Get the damage of the attacker
        int dmg = attacker.GetComponent<MobController>().Character.Attack;
        // and add the additional damage from the talent
        int totalDamage = dmg + additionalDamage;

        //yield return new WaitForSeconds(5f);
        MobAttack attackerAttack = attacker.GetComponent<MobAttack>();
        MobHealth defenderHealth = defender.GetComponent<MobHealth>();

        Debug.Log("-------->" + attackerAttack.mobData.name + " is attacking " + defenderHealth.mobData.name);

        if (attackerAttack != null || defenderHealth != null)
        {
            // Set the target of the MobAttack to the MobHealth component of the enemy
            attackerAttack.target = defender.GetComponent<MobHealth>();
            // Set the source of the MobHealth to the MobAttack component of the attacker
            defenderHealth.source = attacker.GetComponent<MobHealth>();
            yield return StartCoroutine(attackerAttack.TriggerAttackCoroutine(totalDamage));
        }
        else
        {
            Debug.Log("MobAttack component not found on attacker or MobHealth component not found on defender.");
        }
    }

    IEnumerator ApplyEffects(GameObject character, Phase phase)
    {

        yield return null;
    }

    (int, int, Buff) applyEffect(GameObject character, Phase phase)
    {
        GameCharacter c = character.GetComponent<MobController>().Character;
        // Talent buff
        var (talent_damage, stun, next_buff) = c.Talent(phase);
        Debug.Log("----> " + "Next buff: " + next_buff.Health + " " + next_buff.Attack + " " + next_buff.Absorb);
        // Item buff
        var item_dmg = c.Usage(phase);
        Debug.Log("----> " + "item_dmg: " + item_dmg);
        return (talent_damage + item_dmg, stun, next_buff);
    }

    IEnumerator PostMortem(GameObject attacker, GameObject defender, int dmg)
    {
        //yield return new WaitForSeconds(5f);
        MobAttack attackerAttack = attacker.GetComponent<MobAttack>();
        MobHealth defenderHealth = defender.GetComponent<MobHealth>();

        Debug.Log("-------->" + attackerAttack.mobData.name + " is postmorteming " + defenderHealth.mobData.name);

        if (attackerAttack != null || defenderHealth != null)
        {
            // Set the target of the MobAttack to the MobHealth component of the enemy
            attackerAttack.target = defender.GetComponent<MobHealth>();
            // Set the source of the MobHealth to the MobAttack component of the attacker
            defenderHealth.source = attacker.GetComponent<MobHealth>();
            yield return StartCoroutine(attackerAttack.TriggerPostMortemCoroutine(dmg));
        }
        else
        {
            Debug.Log("MobAttack component not found on attacker or MobHealth component not found on defender.");
        }
    }


    (int, Buff) postMortem(GameObject character, GameObject foe)
    {
        GameCharacter c = character.GetComponent<MobController>().Character;
        GameCharacter f = foe.GetComponent<MobController>().Character;
        if (c.IsDead())
        {
            var (damage, stun, next_buff) = applyEffect(character, Phase.OnDeath);
            f.Stun = stun;
            f.TakeDamage(damage);
            return (damage, next_buff);
        }
        else
        {
            return (0, new Buff());
        }
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
            //allies.RemoveAt(0);
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
            //enemies.RemoveAt(0);
        }

        yield return null;
    }

    bool IsFirstMobDead(List<GameObject> mobs)
    {
        if (mobs != null && mobs.Count > 0)
        {
            MobHealth mobHealth = mobs[0].GetComponent<MobHealth>();
            Debug.Log(mobHealth.Health);
            return mobHealth != null && mobHealth.Health <= 0;
        }
        return false;
    }
}
