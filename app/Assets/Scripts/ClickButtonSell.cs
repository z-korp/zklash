using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System;

public class ClickButtonSell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Sell Mob?");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("En fait non!");
    }

    public void OnClickSell(GameObject draggedObject)
    {
        Debug.Log("Selling mob: " + draggedObject.name);
        uint teamId = PlayerData.Instance.GetTeamId();
        string entity = TeamManager.instance.GetEntityFromTeam(draggedObject);
        if (entity == "")
        {
            Debug.Log("Entity not found.");
            return;
        }
        Character character = GameManager.Instance.worldManager.Entity(entity).GetComponent<Character>();
        StartCoroutine(TxCoroutines.Instance.ExecuteSell(teamId, character.id));
    }
}
