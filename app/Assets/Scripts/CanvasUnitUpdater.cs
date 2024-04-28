using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasUnitUpdater : MonoBehaviour
{
    private Image ribbon;
    private Image ribbonSimple;

    void Start()
    {
        ribbon = transform.Find("Ribbon").GetComponent<Image>();
        ribbonSimple = transform.Find("RibbonSimple").GetComponent<Image>();

        ToggleRibbons(false);
    }

    void Update()
    {
        // Toggle ribbons based on user input
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleRibbons(true);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleRibbons(false);
        }
    }

    public void ToggleRibbons(bool showRibbon)
    {
        if (ribbon == null || ribbonSimple == null)
        {
            Debug.Log("Ribbon or RibbonSimple not found on the GameObject.", this);
            return;
        }
        SetActiveRibbon(showRibbon);
        SetActiveRibbonSimple(!showRibbon);

        if (!showRibbon)
        {
            transform.Find("ItemInfo").gameObject.SetActive(false);
        }
        else
        {
            transform.Find("ItemInfo").gameObject.SetActive(true);
        }
    }

    private void SetActiveRibbon(bool isActive)
    {
        ribbon.gameObject.SetActive(isActive);
    }

    private void SetActiveRibbonSimple(bool isActive)
    {
        ribbonSimple.gameObject.SetActive(isActive);
    }
}
