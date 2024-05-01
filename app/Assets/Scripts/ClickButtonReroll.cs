using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System;

public class ClickButtonReroll : MonoBehaviour
{
    public void OnClickReroll()
    {
        //StartCoroutine(ContractActions.instance.WaitForAllTransactionsCoroutine());
        uint teamId = PlayerData.Instance.GetTeamId();
        StartCoroutine(TxCoroutines.Instance.ExecuteReroll(teamId));
    }
}
