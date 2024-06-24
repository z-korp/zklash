using System.Collections;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    public GameObject canvasInfo;
    public GameObject canvasShopInfo;
    public GameObject canvasInterStep;
    public GameObject canvasWaitForTransaction;

    public bool debugOn = true;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of CanvasManager found!");
            return;
        }
        instance = this;
    }

    public void ToggleCanvasInfo()
    {
        canvasInfo.SetActive(!canvasInfo.activeSelf);
    }

    public void HideOrShowUserStatsInfo(bool show)
    {
        canvasInfo.transform.Find("PanelRibbon").gameObject.SetActive(show);
    }

    public void ToggleCanvasShopInfo()
    {
        canvasShopInfo.SetActive(!canvasShopInfo.activeSelf);
    }

    public void ToggleCanvasInterStep(bool victory = true)
    {
        canvasInterStep.SetActive(!canvasInterStep.activeSelf);
        // TBD : Update is good value
        if (canvasInterStep.activeSelf)
        {
            canvasInterStep.GetComponent<CanvasInterStep>().UpdateTrophysDisplay((int)PlayerInfoUI.instance.getTrophies());
            canvasInterStep.GetComponent<CanvasInterStep>().UpdateHeartsDisplay((int)PlayerInfoUI.instance.getLifes());
            canvasInterStep.GetComponent<CanvasInterStep>().ToggleRibbonVictoryDefeat(victory);
        }
    }

    public void ToggleCanvases()
    {
        ToggleCanvasInfo();
        ToggleCanvasShopInfo();
    }

    public void ToggleSellRerollButton()
    {
        Transform btnReroll = canvasInfo.transform.Find("BtnReroll");
        Transform btnSell = canvasInfo.transform.Find("BtnSell");
        if (btnReroll != null)
            btnReroll.gameObject.SetActive(!btnReroll.gameObject.activeSelf);
        if (btnSell != null)
            btnSell.gameObject.SetActive(!btnSell.gameObject.activeSelf);
    }

    public void ToggleCanvasForDuration(float duration)
    {
        if (!debugOn)
            StartCoroutine(ToggleCanvasCoroutine(duration));
    }

    private IEnumerator ToggleCanvasCoroutine(float duration)
    {
        canvasWaitForTransaction.SetActive(true);
        yield return new WaitForSeconds(duration);
        canvasWaitForTransaction.SetActive(false);
    }
}
