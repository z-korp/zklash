using UnityEngine;
using UnityEngine.UI;

public class CanvasInterStep : MonoBehaviour
{
    public GameObject ribbonVictory;
    public GameObject ribbonDefeat;
    public Transform heartsContainer;
    public Transform trophysContainer;
    public Image heartPrefab;
    public Image trophyPrefab;

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
        for (int i = 0; i < remainingLives; i++)
        {
            Image heart = Instantiate(heartPrefab, heartsContainer);
            //heart.gameObject.SetActive(i < remainingLives);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)heartsContainer);
    }

    public void UpdateTrophysDisplay(int trophyCount)
    {
        foreach (Transform child in trophysContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < trophyCount; i++)
        {
            Image trophy = Instantiate(trophyPrefab, trophysContainer);
            //trophy.gameObject.SetActive(i < remainingLives); // Active le cœur seulement s'il reste des vies
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)trophysContainer);
    }
}
