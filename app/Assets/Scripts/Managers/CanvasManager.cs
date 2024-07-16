using System.Collections;
using UnityEngine;

public class CanvasManager : Singleton<CanvasManager>
{
    public GameObject canvasInfo;
    public GameObject canvasShopInfo;
    public GameObject canvasInterStep;

    public GameObject canvasWinLoose;

    private Transform btnReroll;
    private Transform btnSell;

    private void Start()
    {

        btnReroll = canvasInfo.transform.Find("BtnReroll");
        if (btnReroll == null)
        {
            Debug.LogError("BtnReroll not found in CanvasManager");
        }
        btnSell = canvasInfo.transform.Find("BtnSell");
        if (btnSell == null)
        {
            Debug.LogError("BtnSell not found in CanvasManager");
        }
    }

    public void ToggleCanvasInfo()
    {
        canvasInfo.SetActive(!canvasInfo.activeSelf);
    }

    public void HideUserStatsInfo()
    {
        canvasInfo.transform.Find("PanelRibbon").gameObject.SetActive(false);
    }

    public void ShowUserStatsInfo()
    {
        canvasInfo.transform.Find("PanelRibbon").gameObject.SetActive(true);
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

    public void ShowSellButton()
    {
        btnReroll.gameObject.SetActive(false);
        btnSell.gameObject.SetActive(true);
    }

    public void ShowRerollButton()
    {
        btnReroll.gameObject.SetActive(true);
        btnSell.gameObject.SetActive(false);
    }

    public void ShowCanvasWinOrLoose(bool victory = true)
    {
        if (victory)
            canvasWinLoose.GetComponent<CanvasWinLoose>().ToggleWinPanel();
        else
            canvasWinLoose.GetComponent<CanvasWinLoose>().ToggleLoosePanel();
    }
}
