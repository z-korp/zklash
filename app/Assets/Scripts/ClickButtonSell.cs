using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System;

public class ClickButtonSell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Sell Mob !");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("En fait non!");
    }
    public void OnClickSell(uint character_id)
    {
        //ContractActions.instance.TriggerSell(character_id);
        uint teamId = PlayerData.Instance.GetTeamId();
        uint character_id = 0; // TBD
        StartCoroutine(TxCoroutines.Instance.ExecuteSell(teamId, character_id));
    }
}
