using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System;

public class ClickButtonSell : MonoBehaviour
{
    public void OnClickSell()
    {
        //ContractActions.instance.TriggerSell(character_id);
        uint teamId = PlayerData.Instance.GetTeamId();
        uint character_id = 0; // TBD
        StartCoroutine(TxCoroutines.Instance.ExecuteSell(teamId, character_id));
    }
}
