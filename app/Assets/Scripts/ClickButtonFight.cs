using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickButtonFight : MonoBehaviour
{
    public void OnClickFight()
    {
        CameraMovement.instance.MoveCameraToFight();
        TeamManager.instance.MoveTeam();
    }
}
