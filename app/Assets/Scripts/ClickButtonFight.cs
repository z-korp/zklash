using UnityEngine;
using System.Linq;
using System;

public class ClickButtonFight : MonoBehaviour
{

    public void OnClickFight()
    {
        CameraMovement.instance.MoveCameraToFight();
        CanvasManager.instance.ToggleCanvases();
        TeamManager.instance.MoveTeam();

        // Empty allies list from previous battle
        BattleManager.instance.allies.Clear();

        if (BattleManager.instance.allies.Count == 0)
        {
            Debug.Log("------> No allies found");

            // Reversing the TeamSpots array in place
            TeamSpot[] reversedTeamSpots = new TeamSpot[TeamManager.instance.TeamSpots.Length];
            Array.Copy(TeamManager.instance.TeamSpots, reversedTeamSpots, TeamManager.instance.TeamSpots.Length);
            Array.Reverse(reversedTeamSpots);

            // Sorting the dictionary by descending key and converting it to a list of GameObjects
            BattleManager.instance.allies = reversedTeamSpots
                .Where(spot => !spot.IsAvailable)  // Filter spots where IsAvailable is false
                .Select(spot => spot.mob)  // Select the mob GameObject from each spot
                .ToList();

            // Now allies is sorted by the descending order of keys from the original dictionary
            // You can debug or work with 'allies' list now
        }

        BattleManager.instance.InstanciatePlayerTeam();
        BattleManager.instance.InstantiateEnemyTeam();
    }
}
