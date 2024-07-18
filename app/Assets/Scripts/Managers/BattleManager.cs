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

public struct EffectResult
{
    public int TalentDamage;
    public int ItemDamage;
    public int Stun;
    public Buff NextBuff;
}

public enum BattleMode
{
    Normal,
    Test
}

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public BattleMode battleMode;

    public GameObject[] unitPrefabs;

    public GameObject dynamitePrefab;
    public GameObject stonePrefab;
    public GameObject arrowPrefab;
    public GameObject powerUpPrefab;
    public GameObject attackUpPrefab;
    public GameObject healthUpPrefab;

    public ItemData[] itemDataArray;

    public List<GameObject> allySpots = new List<GameObject>();
    public List<GameObject> enemySpots = new List<GameObject>();

    public List<CharacterSetup> alliesSetup;
    public List<CharacterSetup> enemiesSetup;

    public List<GameObject> allies = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();

    private int _maxTrophy = 10;
    private int _minLife = 0;

    private bool battleStarted = false;

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
        if (battleMode == BattleMode.Test)
        {
            LoadBattleSetup(14);
        }
    }

    void Update()
    {
        if (battleMode == BattleMode.Test && !battleStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartBattle();
        }
    }

    public void LoadBattleSetup(int index)
    {
        Debug.Log("LoadBattleSetup: " + BattleSetups.setups.Count);
        if (index >= 0 && index < BattleSetups.setups.Count)
        {
            var setup = BattleSetups.setups[index];
            alliesSetup = setup.alliesSetup;
            enemiesSetup = setup.enemiesSetup;

            InstantiateTeams();
        }
        else
        {
            Debug.LogError("Invalid battle setup index: " + index);
        }
    }

    private void InstantiateTeams()
    {
        DestroyGameObjectFromList(allies);
        DestroyGameObjectFromList(enemies);

        InstanciateTeam(allies, alliesSetup, allySpots, Orientation.Right);
        InstanciateTeam(enemies, enemiesSetup, enemySpots, Orientation.Left);
    }

    private void StartBattle()
    {
        battleStarted = true;
        StartCoroutine(Battle(allies, enemies));
    }

    private GameObject CreateMob(CharacterSetup setup, GameObject spot, Orientation orientation)
    {
        Debug.Log("CreateMob: " + setup.role + " --- " + setup.level + " --- " + setup.item);
        if (setup.role == Role.None || setup.role.ToString() == "None")
            return null;

        string roleName = PrefabMappings.NameToRoleMap[setup.role];
        GameObject prefab = PrefabUtils.FindPrefabByName(unitPrefabs, roleName);
        if (prefab == null || spot == null)
            return null;

        Debug.Log("CreateMob: " + setup.role + " " + setup.level + " " + setup.item + " " + spot.name + " " + orientation.ToString() + " " + prefab.name);

        GameObject mobObject = Instantiate(prefab, spot.transform.position, Quaternion.identity);
        Debug.Log("MobObject: " + mobObject.name);
        mobObject.GetComponent<MobOrientation>().SetOrientation(orientation);
        Debug.Log("MobObject: " + mobObject.GetComponent<Animator>());
        TimeScaleController.Instance.AddAnimator(mobObject.GetComponent<Animator>());

        GameCharacter character = new GameCharacter(setup.role, setup.level, setup.item);
        mobObject.GetComponent<MobController>().ConfigureCharacter(character);

        string itemName = PrefabMappings.NameToItemDataMap[setup.item];
        Debug.Log("ItemName: " + itemName);
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
        Debug.Log("-----------> mobs: " + mobs.Count + " --- mobSetups: " + mobSetups.Count + " --- spots: " + spots.Count + " --- orientation: " + orientation);
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

    private void ExecuteAfterBattle(bool result)
    {
        if ((int)PlayerInfoUI.instance.getLifes() == _maxTrophy)
            CanvasManager.instance.ShowCanvasWinOrLoose(result);
        else if ((int)PlayerInfoUI.instance.getLifes() == _minLife)
            CanvasManager.instance.ShowCanvasWinOrLoose(result);
        else
        {
            //CameraMovement.instance.MoveCameraToShop();
            TeamManager.instance.ResetStatCharacter();
            TeamManager.instance.TPTeamToShop();
            CanvasManager.instance.ToggleCanvasInterStep(result);
        }
    }

    private void LaunchProjectile(Vector3 position, Transform target, GameObject projectilePrefab, SoundEffect soundOnHit, char size = '0')
    {
        Debug.Log("LaunchProjectile: " + position + " --- " + target + " --- " + projectilePrefab + " --- " + size);
        var projectile = Instantiate(projectilePrefab, position, Quaternion.identity);
        // TBD dirty hack to set if stone or not, if set to 0, it will not be a stone
        if (size != '0')
        {
            projectile.GetComponent<StoneSizeController>().SetStoneSize(size);
        }
        projectile.GetComponent<ProjectileParabolic>().Initialize(target, soundOnHit);
        TimeScaleController.Instance.AddAnimator(projectile.GetComponent<Animator>());
    }

    private void PlayPowerUp(Vector3 position)
    {
        var powerUp = Instantiate(powerUpPrefab, position, Quaternion.identity);
        TimeScaleController.Instance.AddAnimator(powerUp.GetComponentInChildren<Animator>());
    }

    private void PlayAttackUp(Vector3 position)
    {
        var attackUp = Instantiate(attackUpPrefab, position, Quaternion.identity);
        TimeScaleController.Instance.AddAnimator(attackUp.GetComponentInChildren<Animator>());
    }

    private void PlayHealthUp(Vector3 position)
    {
        var attackUp = Instantiate(healthUpPrefab, position, Quaternion.identity);
        TimeScaleController.Instance.AddAnimator(attackUp.GetComponentInChildren<Animator>());
    }

    private void PlayDamageEffects(Role role, Vector3 position, Transform target)
    {
        switch (role)
        {
            case Role.Dynamoblin:
                LaunchProjectile(position, target, dynamitePrefab, SoundEffect.Explosion);
                // TBD : add good death rattle
                break;
            case Role.Bowman:
                LaunchProjectile(position, target, arrowPrefab, SoundEffect.Stun);
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
                PlayAttackUp(position);
                break;
            case Role.Bomboblin:
                PlayHealthUp(position);
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
            ApplyEffect(team1[0], Phase.OnDispatch);
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
            ApplyEffect(team2[0], Phase.OnDispatch);
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
        Debug.Log("[START DUEL]============================================================");
        Debug.Log("Dueling");

        // Apply effects and calculate damage
        EffectResult char1Effect = ApplyEffect(char1, Phase.OnFight);
        EffectResult char2Effect = ApplyEffect(char2, Phase.OnFight);

        // Apply stun effects
        char1.GetComponent<MobController>().Character.Stun = char1Effect.Stun;
        char2.GetComponent<MobController>().Character.Stun = char2Effect.Stun;

        // Perform attacks
        yield return PerformItemAttacks(char1, char2, char1Effect, char2Effect);
        if (!IsCharacterDead(char1) && !IsCharacterDead(char2))
            yield return PerformAttacks(char1, char2, char1Effect, char2Effect);

        yield return new WaitForSeconds(0.1f);

        // Check deaths and apply post-mortem effects
        bool char1WasAlive = !IsCharacterDead(char1);
        bool char2WasAlive = !IsCharacterDead(char2);

        yield return CheckDeathAndApplyEffects(char1, char2, team1, team2);
        yield return CheckDeathAndApplyEffects(char2, char1, team2, team1);

        // If char1 was alive before but is now dead, and char2 is dead, apply char1's post-mortem effect
        if (char1WasAlive && IsCharacterDead(char1) && IsCharacterDead(char2))
            yield return CheckDeathAndApplyEffects(char1, char2, team1, team2);

        // Trigger death animations
        yield return TriggerDeathAnimations(char1, char2);

        bool isChar1Dead = IsCharacterDead(char1);
        bool isChar2Dead = IsCharacterDead(char2);

        if (isChar1Dead || isChar2Dead)
        {
            if (battleMode == BattleMode.Test)
                yield return WaitForKey();
            Debug.Log("[END DUEL]============================================================");
            yield break;
        }

        if (battleMode == BattleMode.Test)
            yield return WaitForKey();
        Debug.Log("[END CONTINUE DUEL]============================================================");

        yield return Duel(char1, char2, team1, team2);
    }

    private IEnumerator PerformItemAttacks(GameObject char1, GameObject char2, EffectResult effectChar1, EffectResult effectChar2)
    {
        int damage1 = effectChar1.ItemDamage;
        int damage2 = effectChar2.ItemDamage;
        var item1 = char1.GetComponent<MobItem>().previousItem;
        var item2 = char2.GetComponent<MobItem>().previousItem;

        bool projectile1Launched = LaunchProjectileIfNeeded(char1, char2, damage1, item1);
        bool projectile2Launched = LaunchProjectileIfNeeded(char2, char1, damage2, item2);

        bool anyProjectileLaunched = projectile1Launched || projectile2Launched;

        if (anyProjectileLaunched)
            yield return new WaitForSeconds(0.8f);

        yield return StartCoroutine(ApplyDamageSimultaneously(char1, char2, damage1, damage2));

        if (anyProjectileLaunched)
            yield return new WaitForSeconds(0.8f);
    }

    private bool LaunchProjectileIfNeeded(GameObject attacker, GameObject target, int damage, ItemData item)
    {
        if (damage > 0)
        {
            LaunchProjectile(attacker.transform.position, target.transform, stonePrefab, SoundEffect.Swap, item.size);
            return true;
        }
        return false;
    }

    private IEnumerator ApplyDamageSimultaneously(GameObject char1, GameObject char2, int damage1, int damage2)
    {
        Coroutine dmg1 = damage1 > 0 ? StartCoroutine(char2.GetComponent<MobHealth>().TakeDamage(damage1)) : null;
        Coroutine dmg2 = damage2 > 0 ? StartCoroutine(char1.GetComponent<MobHealth>().TakeDamage(damage2)) : null;

        if (dmg1 != null) yield return dmg1;
        if (dmg2 != null) yield return dmg2;
    }

    private IEnumerator PerformAttacks(GameObject char1, GameObject char2, EffectResult effectChar1, EffectResult effectChar2)
    {
        Coroutine attack1 = StartCoroutine(Attack(char1, char2, effectChar1.TalentDamage));
        Coroutine attack2 = StartCoroutine(Attack(char2, char1, effectChar2.TalentDamage));
        yield return attack1;
        yield return attack2;
    }

    private IEnumerator CheckDeathAndApplyEffects(GameObject attacker, GameObject defender, List<GameObject> attackerTeam, List<GameObject> defenderTeam)
    {
        if (IsCharacterDead(attacker))
        {
            EffectResult effects = ApplyEffect(attacker, Phase.OnDeath);
            // The pumpkin save the character, so let's check if it is really dead after the item effect
            if (IsCharacterDead(attacker))
            {
                yield return ApplyPostMortemEffects(attacker, defender, attackerTeam, effects);

                if (attackerTeam.Count > 1)
                    attackerTeam[1].GetComponent<MobController>().Character.ApplyBuff(effects.NextBuff);
            }
        }
    }

    private IEnumerator ApplyPostMortemEffects(GameObject attacker, GameObject defender, List<GameObject> attackerTeam, EffectResult effects)
    {
        PlayDamageEffects(attacker.GetComponent<MobController>().Character.RoleInterface, attacker.transform.position, defender.transform);
        if (attackerTeam.Count > 1)
            PlayBuffEffects(attacker.GetComponent<MobController>().Character.RoleInterface, attackerTeam[1].transform.position);

        yield return new WaitForSeconds(0.8f);

        int dmg = effects.ItemDamage + effects.TalentDamage;
        if (dmg > 0)
            yield return StartCoroutine(PostMortem(attacker, defender, dmg));

        if (effects.Stun > 0)
            defender.GetComponent<MobController>().Character.Stun = effects.Stun;
    }

    private IEnumerator TriggerDeathAnimations(GameObject char1, GameObject char2)
    {
        Coroutine dead1 = IsCharacterDead(char1) ? StartCoroutine(char1.GetComponent<MobHealth>().TriggerDie()) : null;
        Coroutine dead2 = IsCharacterDead(char2) ? StartCoroutine(char2.GetComponent<MobHealth>().TriggerDie()) : null;

        yield return dead1;
        yield return dead2;
    }

    private bool IsCharacterDead(GameObject character)
    {
        return character.GetComponent<MobHealth>().Health <= 0;
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
            yield break;

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

    IEnumerator ItemEffect(GameObject character)
    {
        Debug.Log("wwwwwwwwwwwwwwwwwwwwwww Item effect");
        yield return character.GetComponent<MobAttack>().BlinkPowerUp();
    }

    // Main ApplyEffect function
    EffectResult ApplyEffect(GameObject character, Phase phase)
    {
        var talentEffect = ApplyTalentEffect(character, phase);
        int itemDamage = ApplyItemEffect(character, phase);

        return new EffectResult
        {
            TalentDamage = talentEffect.TalentDamage,
            ItemDamage = itemDamage,
            Stun = talentEffect.Stun,
            NextBuff = talentEffect.NextBuff
        };
    }

    // Function to apply talent effects
    (int TalentDamage, int Stun, Buff NextBuff) ApplyTalentEffect(GameObject character, Phase phase)
    {
        GameCharacter c = character.GetComponent<MobController>().Character;
        var (talentDamage, stun, nextBuff) = c.Talent(phase);

        Debug.Log($"----> Next buff: Health={nextBuff.Health}, Attack={nextBuff.Attack}, Absorb={nextBuff.Absorb}");

        return (talentDamage, stun, nextBuff);
    }

    // Function to apply item effects
    int ApplyItemEffect(GameObject character, Phase phase)
    {
        GameCharacter c = character.GetComponent<MobController>().Character;
        int itemDamage = c.Usage(phase);

        string itemName = PrefabMappings.NameToItemDataMap[c.Item.GetItemType()];
        if (itemName != "None")
        {
            ItemData item = PrefabUtils.FindScriptableByName(itemDataArray, itemName);
            character.GetComponent<MobItem>().item = item;
        }
        else
        {
            character.GetComponent<MobItem>().item = null;
        }

        Debug.Log($"----> item_dmg: {itemDamage}");
        return itemDamage;
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