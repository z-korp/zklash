using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System;

public class ClickButtonReroll : MonoBehaviour
{
    public void OnClickReroll()
    {
        if (ContractActions.instance != null)
        {
            StartCoroutine(ContractActions.instance.WaitForAllTransactionsCoroutine());
        }
        else
        {
            Debug.LogError("ContractActions instance is not available.");
        }
    }
}
