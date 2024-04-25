using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class ClickButtonFight : MonoBehaviour
{
    public GameObject canvasInfo;
    public GameObject canvasShopInfo;

    public void ToggleCanvases()
    {
        canvasInfo.SetActive(!canvasInfo.activeSelf);
        canvasShopInfo.SetActive(!canvasShopInfo.activeSelf);
    }
    public void OnClickFight()
    {
        CameraMovement.instance.MoveCameraToFight();
        ToggleCanvases();
        TeamManager.instance.MoveTeam();

        if (BattleManagerTest.instance.allies.Count == 0)
        {
            Debug.Log("------> No allies found");
            TeamManager.instance.PrintDictionary(TeamManager.instance.team);

            // Sorting the dictionary by descending key and converting it to a list of GameObjects
            BattleManagerTest.instance.allies = TeamManager.instance.team.OrderByDescending(pair => pair.Key)
                         .Select(pair => pair.Value)
                         .ToList();

            // Now allies is sorted by the descending order of keys from the original dictionary
            // You can debug or work with 'allies' list now
            foreach (var ally in BattleManagerTest.instance.allies)
            {
                Debug.Log(ally.name);
            }
        }
    }
}
