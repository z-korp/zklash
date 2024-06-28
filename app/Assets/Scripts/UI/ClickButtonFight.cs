using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using zKlash.Game.Roles;
using zKlash.Game.Items;

public class ClickButtonFight : MonoBehaviour
{
    public void OnClickFight()
    {
        if (TeamManager.instance.CountMobInTeam() < 1)
        {
            DialogueManager.Instance.ShowDialogueForDuration("Even heroes need an army!", 2f);
            return;
        }
        CanvasManager.instance.HideOrShowUserStatsInfo(false);
        StartCoroutine(FightSequence());
        TimeScaleController.Instance.UpdateAnimatorList();
        TimeScaleController.Instance.ApplySpeed();
    }

    private IEnumerator FightSequence()
    {
        if (!PrepareAllies(out uint order))
            yield break;

        yield return StartCoroutine(StartBattle(order));

        if (!PrepareEnemies(out List<CharacterSetup> foeSetups))
            yield break;

        SetupBattlefield(foeSetups);
        FinalizeSetup();
    }

    private bool PrepareAllies(out uint order)
    {
        var spots = TeamManager.instance.TeamSpots.Where(spot => !spot.IsAvailable).ToArray();
        if (spots.Length == 0)
        {
            Debug.Log("No allies found");
            order = 0;
            return false;
        }

        order = PackCharacterIDs(spots);
        Debug.Log("Packed Order: " + $"0x{order:X}");
        return true;
    }

    private uint PackCharacterIDs(TeamSpot[] spots)
    {
        uint packed = spots.Select(spot => GameManager.Instance.worldManager.Entity(spot.Entity).GetComponent<Character>().id)
                           .Aggregate(0u, (current, id) => (current << 8) | id);
        return packed;
    }

    private IEnumerator StartBattle(uint order)
    {
        string teamEntity = PlayerData.Instance.teamEntity;
        var team = GameManager.Instance.worldManager.Entity(teamEntity).GetComponent<Team>();
        Debug.Log($"Team id: {team.id}, Registry id: {team.registry_id}");
        yield return StartCoroutine(TxCoroutines.Instance.ExecuteStartBattle(team.id, order));
        yield return new WaitForSeconds(1.0f);
        CanvasManager.instance.ToggleCanvasForDuration(2.0f);
        Debug.Log($"Foe squad id: {team.foe_squad_id}, Registry id: {team.registry_id}");
    }

    private bool PrepareEnemies(out List<CharacterSetup> foeSetups)
    {
        string teamEntity = PlayerData.Instance.teamEntity;
        var team = GameManager.Instance.worldManager.Entity(teamEntity).GetComponent<Team>();
        var foes = GameManager.Instance.GetFoeEntities(team.registry_id, team.foe_squad_id);

        if (foes.Count == 0)
        {
            Debug.Log("No foeEntities found");
            foeSetups = null;
            return false;
        }

        foeSetups = foes.Select(foeEntity => GameManager.Instance.worldManager.Entity(foeEntity).GetComponent<Foe>())
                        .Select(foe => new CharacterSetup { role = (Role)foe.role, level = foe.level, item = (Item)foe.item })
                        .ToList();

        foeSetups.ForEach(foe => Debug.Log($"Foe role: {foe.role} Level: {foe.level} Item: {foe.item}"));
        return true;
    }

    private void SetupBattlefield(List<CharacterSetup> foeSetups)
    {
        // Enemies
        BattleManager.instance.DestroyGameObjectFromList(BattleManager.instance.enemies);
        BattleManager.instance.InstanciateTeam(BattleManager.instance.enemies, foeSetups, BattleManager.instance.enemySpots, Orientation.Left);

        // Allies
        //BattleManager.instance.DestroyGameObjectFromList(BattleManager.instance.allies);
        //var reversedTeamSpots = TeamManager.instance.TeamSpots.Reverse().ToArray();
        TeamManager.instance.ReorganizeTeamSpots();
        var teamSpots = TeamManager.instance.TeamSpots.ToArray();
        BattleManager.instance.allies = teamSpots.Where(spot => !spot.IsAvailable).Select(spot => spot.Mob).ToList();
        BattleManager.instance.InstanciateTeam(BattleManager.instance.allies, BattleManager.instance.alliesSetup, BattleManager.instance.allySpots, Orientation.Right);
        TeamManager.instance.SaveInfoMobBeforeFight();
    }

    private void FinalizeSetup()
    {
        CanvasManager.instance.HideOrShowUserStatsInfo(true);
        CameraMovement.instance.MoveCameraToFight();
        CanvasManager.instance.ToggleCanvases();
        TeamManager.instance.MoveTeam();

        // Refresh shop
        EventManager.RefreshShop();
    }
}
