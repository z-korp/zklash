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

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public GameObject[] unitPrefabs;

    public GameObject dynamitePrefab;
    public GameObject arrowPrefab;
    public GameObject powerUpPrefab;

    public GameObject attackUpPrefab;

    public ItemData[] itemDataArray;

    public List<GameObject> allySpots = new List<GameObject>();
    public List<GameObject> enemySpots = new List<GameObject>();

    public List<CharacterSetup> alliesSetup;
    public List<CharacterSetup> enemiesSetup;

    public List<GameObject> allies = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of BattleManager found!");
            return;
        }
        instance = this;
    }

    private void OnEnable()
    {
        EventManager.OnStartBattle += StartBattle;
    }

    private void OnDisable()
    {
        EventManager.OnStartBattle -= StartBattle;
    }

    void Start()
    {
        /*for (int i = 0; i < alliesSetup.Count; i++)
        {
            var ally = alliesSetup[i];

            GameCharacter character = new GameCharacter(ally.role, ally.level, ally.item);
            allyCharacters.Add(character);

            if (ally.role == Role.None)
            {
                continue;
            }

            var name = PrefabMappings.NameToRoleMap[ally.role];
            var prefab = PrefabUtils.FindPrefabByName(unitPrefabs, name);

            if (prefab != null && allySpots[i] != null && name != "None")
            {
                GameObject allyObject = Instantiate(prefab, allySpots[i].transform.position, Quaternion.identity);
                allyObject.GetComponent<MobOrientation>().SetOrientation(Orientation.Right);
                allyObject.GetComponent<MobController>().ConfigureCharacter(character);

                var itemName = PrefabMappings.NameToItemDataMap[ally.item];
                if (itemName != "None")
                {
                    var item = PrefabUtils.FindScriptableByName(itemDataArray, itemName);
                    allyObject.GetComponent<MobItem>().item = item;
                }

                HideXpCanvas(allyObject);

                allies.Add(allyObject);
            }
        }


        for (int i = 0; i < enemiesSetup.Count; i++)
        {
            var enemy = enemiesSetup[i];

            GameCharacter character = new GameCharacter(enemy.role, enemy.level, enemy.item);
            enemyCharacters.Add(character);

            if (enemy.role == Role.None)
            {
                continue;
            }

            var name = PrefabMappings.NameToRoleMap[enemy.role];
            var prefab = PrefabUtils.FindPrefabByName(unitPrefabs, name);

            if (prefab != null && enemySpots[i] != null)
            {
                GameObject enemyObject = Instantiate(prefab, enemySpots[i].transform.position, Quaternion.identity);
                enemyObject.GetComponent<MobOrientation>().SetOrientation(Orientation.Left);
                enemyObject.GetComponent<MobController>().ConfigureCharacter(character);

                var itemName = PrefabMappings.NameToItemDataMap[enemy.item];
                if (itemName != "None")
                {
                    var item = PrefabUtils.FindScriptableByName(itemDataArray, itemName);
                    enemyObject.GetComponent<MobController>().Character.Equip(item.type);
                    enemyObject.GetComponent<MobItem>().item = item;
                }

                HideXpCanvas(enemyObject);

                enemies.Add(enemyObject);
            }
        }*/
    }

    void Update()
    {
    }

    private void StartBattle()
    {
        StartCoroutine(Battle(allies, enemies));
    }

    private GameObject CreateMob(CharacterSetup setup, GameObject spot, Orientation orientation)
    {
        if (setup.role == Role.None || setup.role.ToString() == "None")
            return null;

        string roleName = PrefabMappings.NameToRoleMap[setup.role];
        GameObject prefab = PrefabUtils.FindPrefabByName(unitPrefabs, roleName);
        if (prefab == null || spot == null)
            return null;

        GameObject mobObject = Instantiate(prefab, spot.transform.position, Quaternion.identity);
        mobObject.GetComponent<MobOrientation>().SetOrientation(orientation);
        TimeScaleController.Instance.AddAnimator(mobObject.GetComponent<Animator>());

        GameCharacter character = new GameCharacter(setup.role, setup.level, setup.item);
        mobObject.GetComponent<MobController>().ConfigureCharacter(character);

        string itemName = PrefabMappings.NameToItemDataMap[setup.item];
        if (itemName != "None")
        {
            ItemData item = PrefabUtils.FindScriptableByName(itemDataArray, itemName);
            mobObject.GetComponent<MobController>().Character.Equip(item.type);
            mobObject.GetComponent<MobItem>().item = item;
        }

        HideXpCanvas(mobObject);

        return mobObject;
    }

    public void InstanciateTeam(List<GameObject> mobs, List<CharacterSetup> mobSetups, List<GameObject> spots, Orientation orientation)
    {
        if (mobs.Count > 0)
            return;

        for (int i = 0; i < mobSetups.Count; i++)
        {
            GameObject mob = CreateMob(mobSetups[i], spots[i], orientation);
            if (mob != null)
                mobs.Add(mob);
        }
    }

    public void DestroyGameObjectFromList(List<GameObject> gameObjectsList)
    {
        // Iterate through the list and destroy each GameObject
        foreach (GameObject obj in gameObjectsList)
        {
            Destroy(obj);
        }

        // Clear the list after destroying all GameObjects
        gameObjectsList.Clear();
    }

    void HideXpCanvas(GameObject go)
    {
        Transform canvasXpTransform = go.transform.Find("CanvasXP"); // Adjust path if nested deeper
        if (canvasXpTransform != null)
        {
            Canvas canvas = canvasXpTransform.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.enabled = false; // Disable rendering for this canvas
            }
            else
            {
                Debug.LogError("Canvas component not found on 'CanvasXP' object.");
            }
        }
        else
        {
            Debug.LogError("CanvasXp GameObject not found.");
        }
    }

    private Buff next_buff1 = new Buff();
    private Buff next_buff2 = new Buff();

    private void ExecuteAfterBattle(bool result)
    {
        //CameraMovement.instance.MoveCameraToShop();
        TeamManager.instance.ResetStatCharacter();
        TeamManager.instance.TPTeamToShop();
        CanvasManager.instance.ToggleCanvases();
        CanvasManager.instance.ToggleCanvasInterStep(result);
    }

    private void LaunchProjectile(Vector3 position, Transform target, GameObject projectilePrefab)
    {
        var dynamite = Instantiate(projectilePrefab, position, Quaternion.identity);
        dynamite.GetComponent<ProjectileParabolic>().Initialize(target);
        TimeScaleController.Instance.AddAnimator(dynamite.GetComponent<Animator>());

    }

    private void PlayPowerUp(Vector3 position)
    {
        var powerUp = Instantiate(powerUpPrefab, position, Quaternion.identity);
        TimeScaleController.Instance.AddAnimator(powerUp.GetComponentInChildren<Animator>());

    }

    private void PlayAttachUp(Vector3 position)
    {
        var attackUp = Instantiate(attackUpPrefab, position, Quaternion.identity);
        TimeScaleController.Instance.AddAnimator(attackUp.GetComponentInChildren<Animator>());

    }

    private void PlayDeathRattleEffects(Role role, Vector3 position, Transform target)
    {
        switch (role)
        {
            case Role.Dynamoblin:
                LaunchProjectile(position, target, dynamitePrefab);
                // TBD : add good death rattle
                break;
            case Role.Bowman:
                LaunchProjectile(position, target, arrowPrefab);
                // TBD : add good death rattle
                break;
        }
    }

    private void PlayBuffEffects(Role role, Vector3 position)
    {
        switch (role)
        {
            case Role.Pawn:
                PlayPowerUp(position);
                break;
            case Role.Torchoblin:
                PlayAttachUp(position);
                break;
        }
    }

    IEnumerator Battle(List<GameObject> team1, List<GameObject> team2)
    {
        Debug.Log("Team 1: " + team1.Count + " --- Team 2: " + team2.Count);
        if (team1[0].GetComponent<MobHealth>().Health <= 0)
        {
            RepositionAllies();
            team1.RemoveAt(0);
            if (team1.Count == 0)
            {
                Debug.Log("Team 1 has lost");
                ExecuteAfterBattle(false);
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
            RepositionEnnemies();
            team2.RemoveAt(0);
            if (team2.Count == 0)
            {
                Debug.Log("Team 2 has lost");
                ExecuteAfterBattle(true);
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

        yield return new WaitForSeconds(2f / TimeScaleController.Instance.speedGame);
        yield return Duel(char1, char2, team1, team2);

        //yield return RepositionTeams();
        //yield return new WaitForSeconds(2f);

        yield return Battle(team1, team2);
    }

    IEnumerator Duel(GameObject char1, GameObject char2, List<GameObject> team1, List<GameObject> team2)
    {
        GameCharacter c1 = char1.GetComponent<MobController>().Character;
        GameCharacter c2 = char2.GetComponent<MobController>().Character;

        Debug.Log("[START DUEL]============================================================");
        Debug.Log("Dueling");

        // [Effect] Apply talent and item buff for char1 and char2
        var item = c1.Item;
        var (talent_dmg1, item_dmg1, stun1, _) = applyEffect(char1, Phase.OnFight);
        int damage1 = talent_dmg1 + item_dmg1;
        /*if (item_dmg1 != 0)
        {
            yield return StartCoroutine(ItemEffect(char1));
        }*/

        var (talent_dmg2, item_dmg2, stun2, _) = applyEffect(char2, Phase.OnFight);
        /*if (item_dmg2 != 0)
        {
            yield return StartCoroutine(ItemEffect(char2));
        }*/
        int damage2 = talent_dmg2 + item_dmg2;

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
            PlayDeathRattleEffects(char1.GetComponent<MobController>().Character.RoleInterface, char1.transform.position, char2.transform);
            if (team1.Count > 1)
                PlayBuffEffects(char1.GetComponent<MobController>().Character.RoleInterface, team1[1].transform.position);

            var (dmg1, buff1) = postMortem(char1, char2);
            if (dmg1 > 0)
                yield return StartCoroutine(PostMortem(char1, char2, dmg1));

            var (dmg2, buff2) = postMortem(char2, char1);
            if (dmg2 > 0)
                yield return StartCoroutine(PostMortem(char2, char1, dmg2));

            next_buff1 = buff1;
            next_buff2 = buff2;
        }
        if (isChar2Dead)
        {


            PlayDeathRattleEffects(char2.GetComponent<MobController>().Character.RoleInterface, char2.transform.position, char1.transform);
            if (team2.Count > 1)
                PlayBuffEffects(char2.GetComponent<MobController>().Character.RoleInterface, team2[1].transform.position);

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
            //yield return WaitForKey();
            Debug.Log("[END DUEL]============================================================");
            yield break;
        }

        //yield return WaitForKey();
        Debug.Log("[END CONTINUE DUEL]============================================================");

        yield return Duel(char1, char2, team1, team2);
    }

    IEnumerator WaitForKey()
    {
        yield return null;
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
    }

    IEnumerator Attack(GameObject attacker, GameObject defender, int additionalDamage)
    {
        // Get the damage of the attacker
        int dmg = attacker.GetComponent<MobController>().Character.Damage;
        // and add the additional damage from the talent
        int totalDamage = dmg + additionalDamage;

        int stun = attacker.GetComponent<MobController>().Character.Stun;

        if (totalDamage <= 0 || stun > 0)
        {
            yield break;
        }

        //yield return new WaitForSeconds(5f);
        MobAttack attackerAttack = attacker.GetComponent<MobAttack>();
        MobHealth defenderHealth = defender.GetComponent<MobHealth>();

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

    IEnumerator ApplyEffect(GameObject character, Phase phase)
    {
        yield return null;
    }

    IEnumerator ItemEffect(GameObject character)
    {
        Debug.Log("wwwwwwwwwwwwwwwwwwwwwww Item effect");
        yield return character.GetComponent<MobAttack>().BlinkPowerUp();
    }

    (int, int, int, Buff) applyEffect(GameObject character, Phase phase)
    {
        GameCharacter c = character.GetComponent<MobController>().Character;
        // Talent buff
        var (talent_damage, stun, next_buff) = c.Talent(phase);
        Debug.Log("----> " + "Next buff: " + next_buff.Health + " " + next_buff.Attack + " " + next_buff.Absorb);

        // Item buff
        var item_dmg = c.Usage(phase);

        string itemName = PrefabMappings.NameToItemDataMap[c.Item.GetItemType()];
        if (itemName != "None")
        {
            ItemData item = PrefabUtils.FindScriptableByName(itemDataArray, itemName);
            //c.Equip(item.type);
            character.GetComponent<MobItem>().item = item;
        }
        else
        {
            character.GetComponent<MobItem>().item = null;
        }

        Debug.Log("----> " + "item_dmg: " + item_dmg);
        return (talent_damage, item_dmg, stun, next_buff);
    }

    private void EquipItem(GameObject character, Item itemEnum)
    {
        Debug.Log("Equip item: " + itemEnum);
        GameCharacter c = character.GetComponent<MobController>().Character;
        string itemName = PrefabMappings.NameToItemDataMap[itemEnum];
        if (itemName != "None")
        {
            ItemData item = PrefabUtils.FindScriptableByName(itemDataArray, itemName);
            c.Equip(item.type);
            character.GetComponent<MobItem>().item = item;
        }
    }

    IEnumerator PostMortem(GameObject attacker, GameObject defender, int dmg)
    {
        //yield return new WaitForSeconds(5f);
        MobAttack attackerAttack = attacker.GetComponent<MobAttack>();
        MobHealth defenderHealth = defender.GetComponent<MobHealth>();

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
            var (talent_dmg, item_dmg, stun, next_buff) = applyEffect(character, Phase.OnDeath);
            int damage = talent_dmg + item_dmg;
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
            RepositionAllies();
        }

        if (IsFirstMobDead(enemies))
        {
            RepositionEnnemies();
        }

        yield return null;
    }

    private void RepositionAllies()
    {
        for (int i = 1; i < allies.Count; i++)
        {
            GameObject ally = allies[i]; // Access the ally at index i
            MobMovement mobMovement = ally.GetComponent<MobMovement>();
            if (mobMovement != null)
            {
                mobMovement.speed = 1;
                mobMovement.Move(allySpots[i - 1].transform);
            }
        }
    }
    private void RepositionEnnemies()
    {
        for (int i = 1; i < enemies.Count; i++)
        {
            GameObject ennemy = enemies[i]; // Access the ally at index i
            MobMovement mobMovement = ennemy.GetComponent<MobMovement>();
            if (mobMovement != null)
            {
                mobMovement.Move(enemySpots[i - 1].transform);
            }
        }
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