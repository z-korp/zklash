using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using zKlash.Game.Roles;
using zKlash.Game.Items;
using zklash;
using Dojo.Starknet;
using System;

public class ClickButtonFight : MonoBehaviour
{
    private AudioManager _audioManager;
    private BattleManager _battleManager;
    private CameraMovement _cameraMovement;
    private CanvasManager _canvasManager;
    private DialogueManager _dialogueManager;
    private TeamManager _teamManager;
    private TimeScaleController _timeScaleController;

    private void Start()
    {
        _audioManager = AudioManager.Instance;
        _battleManager = BattleManager.Instance;
        _cameraMovement = CameraMovement.Instance;
        _canvasManager = CanvasManager.Instance;
        _dialogueManager = DialogueManager.Instance;
        _teamManager = TeamManager.Instance;
        _timeScaleController = TimeScaleController.Instance;
    }

    public void OnClickFight()
    {
        _audioManager.SwitchTheme(AudioManager.Theme.Battle);
        if (_teamManager.CountMobInTeam() < 1)
        {
            _dialogueManager.ShowDialogueForDuration("Even heroes need an army!", 2f);
            return;
        }
        _canvasManager.HideUserStatsInfo(); // otherwise the user see the new stats (health, gold) from contract
        StartCoroutine(FightSequence());
        _timeScaleController.UpdateAnimatorList();
        _timeScaleController.ApplySpeed();
    }

    private IEnumerator FightSequence()
    {
        if (!PrepareAllies(out uint order))
            yield break;

        yield return StartCoroutine(StartBattle(order));

        Task<(bool success, List<CharacterSetup> foeSetups, string foeSquadName, uint foeSquadElo)> prepareEnemiesTask = PrepareEnemies();
        yield return new WaitUntil(() => prepareEnemiesTask.IsCompleted);
        var (success, foeSetups, foeSquadName, foeSquadElo) = prepareEnemiesTask.Result;
        if (!success)
            yield break;

        SetupBattlefield(foeSetups, foeSquadName, foeSquadElo);
        FinalizeSetup();
    }

    private bool PrepareAllies(out uint order)
    {
        var spots = _teamManager.TeamSpots.Where(spot => !spot.IsAvailable).ToArray();
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
        Debug.Log($"Foe squad id: {team.foe_squad_id}, Registry id: {team.registry_id}");
    }

    private async Task<(bool success, List<CharacterSetup> foeSetups, string foeSquadName, uint foeSquadElo)> PrepareEnemies()
    {
        Debug.Log($"----------");
        string teamEntity = PlayerData.Instance.teamEntity;
        var team = GameManager.Instance.worldManager.Entity(teamEntity).GetComponent<Team>();
        Debug.Log($"Team id: {team.id}, Registry id: {team.registry_id}");
        Debug.Log($"Team {team}");

        string squadEntity;
        var arr = new FieldElement[] { new FieldElement(team.registry_id), new FieldElement(team.foe_squad_id) };
        squadEntity = StarknetInterop.PoseidonHash(arr);
        squadEntity = new FieldElement(squadEntity).Hex();

        Debug.Log($"----------> hash ${squadEntity}");
        var foeSquadEntity = GameManager.Instance.worldManager.Entity(squadEntity).GetComponent<Squad>();
        string foeSquadName = ShortString.DecodeShortString(foeSquadEntity.name);
        uint foeSquadElo = foeSquadEntity.rating;
        Debug.Log($"Foe squad name: {foeSquadName} ELO {foeSquadElo}");

        var foes = GameManager.Instance.GetFoeEntities(team.registry_id, team.foe_squad_id);
        if (foes.Count == 0)
        {
            Debug.Log("No foeEntities found, let's try querying torii");
            var foeEntities = await Dojo.SynchronizationMaster.Instance.FetchFoeEntities(new FieldElement(team.registry_id), new FieldElement(team.foe_squad_id));
            if (foeEntities.Count == 0)
            {
                Debug.Log("No foeEntities found, aborting");
                return (false, null, foeSquadName, foeSquadElo);
            }

            foreach (var entityId in foeEntities)
            {
                Debug.Log($"Found Foe entity: {entityId}");
            }
        }

        var foeSetups = foes.Select(foeEntity => GameManager.Instance.worldManager.Entity(foeEntity).GetComponent<Foe>())
                            .Select(foe => new CharacterSetup { role = (Role)foe.role, level = foe.level, item = (Item)foe.item })
                            .ToList();

        foeSetups.ForEach(foe => Debug.Log($"Foe role: {foe.role} Level: {foe.level} Item: {foe.item}"));
        return (true, foeSetups, foeSquadName, foeSquadElo);
    }

    private void SetupBattlefield(List<CharacterSetup> foeSetups, string foeSquadName, uint foeSquadElo)
    {
        // Enemies
        _battleManager.DestroyGameObjectFromList(_battleManager.enemies);
        _battleManager.InstanciateTeam(_battleManager.enemies, foeSetups, _battleManager.enemySpots, Orientation.Left);
        _battleManager.SetFoeSquadNameAndElo(foeSquadName, foeSquadElo);
        _canvasManager.ShowFoeSquadNameAndElo();
        // Allies
        //BattleManager.instance.DestroyGameObjectFromList(BattleManager.instance.allies);
        //var reversedTeamSpots = TeamManager.instance.TeamSpots.Reverse().ToArray();
        _teamManager.ReorganizeTeamSpots();
        var teamSpots = _teamManager.TeamSpots.ToArray();
        _battleManager.allies = teamSpots.Where(spot => !spot.IsAvailable).Select(spot => spot.Mob).ToList();
        _battleManager.InstanciateTeam(_battleManager.allies, _battleManager.alliesSetup, _battleManager.allySpots, Orientation.Right);
        _teamManager.SaveInfoMobBeforeFight();
    }

    private void FinalizeSetup()
    {
        _cameraMovement.MoveCameraToFight();
        _canvasManager.ShowUserStatsInfo();
        _canvasManager.ToggleCanvases();
        _teamManager.MoveTeam();

        // Refresh shop
        EventManager.RefreshShop();
    }
}
