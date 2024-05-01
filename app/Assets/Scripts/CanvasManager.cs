using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    public GameObject canvasInfo;
    public GameObject canvasShopInfo;

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

    public void ToggleCanvasShopInfo()
    {
        canvasShopInfo.SetActive(!canvasShopInfo.activeSelf);
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
}
