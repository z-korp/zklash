using UnityEngine;
using UnityEngine.UI;

public class CanvasInterStep : MonoBehaviour
{
    public GameObject ribbonVictory;
    public GameObject ribbonDefeat;
    public GameObject btnNext;
    public Transform heartsContainer;
    public Transform trophysContainer;
    public Image heartPrefab;
    public Image trophyPrefab;
    public Material grayscaleMaterial;

    private void Awake()
    {
        UpdateHeartsDisplay(5);
        UpdateTrophysDisplay(5);
    }

    public void ToggleRibbonVictoryDefeat(bool victory)
    {
        ribbonVictory.SetActive(victory);
        ribbonDefeat.SetActive(!victory);
    }

    public void UpdateHeartsDisplay(int remainingLives)
    {
        foreach (Transform child in heartsContainer)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < 10; i++)
        {
            Image heart = Instantiate(heartPrefab, heartsContainer);
            if (i >= remainingLives)
            {
                heart.material = grayscaleMaterial;
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)heartsContainer);
    }

    public void UpdateTrophysDisplay(int trophyCount)
    {
        foreach (Transform child in trophysContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < 10; i++)
        {
            Image trophy = Instantiate(trophyPrefab, trophysContainer);
            if (i >= trophyCount)
            {
                trophy.material = grayscaleMaterial;
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)trophysContainer);
    }

    public void onClickNext()
    {
        AudioManager.instance.SwitchTheme(AudioManager.Theme.Village);
        CameraMovement.instance.MoveCameraToShop();
        CanvasManager.instance.ToggleCanvases();
    }
}
