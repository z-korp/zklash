using UnityEngine;
using UnityEngine.UI;

public class CanvasInterStep : MonoBehaviour
{
    public GameObject ribbonVictory;
    public GameObject ribbonDefeat;
    public Transform heartsContainer;
    private GridLayoutGroup gridLayout;

    public Image heartPrefab;

    private int maxHearts = 5;

    private void Start()
    {
        gridLayout = heartsContainer.GetComponent<GridLayoutGroup>();
        UpdateHeartsDisplay(5);
    }

    public void ToggleRibbonVictoryDefeat(bool victory)
    {
        ribbonVictory.SetActive(victory);
        ribbonDefeat.SetActive(!victory);
        UpdateHeartsDisplay(3);
    }


    public void UpdateHeartsDisplay(int remainingLives)
    {
        foreach (Transform child in heartsContainer)
        {
            Destroy(child.gameObject);
        }

        gridLayout = heartsContainer.GetComponent<GridLayoutGroup>();
        float containerWidth = gridLayout.cellSize.x * Mathf.Min(maxHearts, remainingLives) +
                                gridLayout.spacing.x * Mathf.Max(0, Mathf.Min(maxHearts - 1, remainingLives - 1));

        RectTransform containerRect = heartsContainer.GetComponent<RectTransform>();
        containerRect.sizeDelta = new Vector2(containerWidth, containerRect.sizeDelta.y);

        // TBD : update heart not working
        for (int i = 0; i < remainingLives; i++)
        {
            Image heart = Instantiate(heartPrefab, heartsContainer);
            heart.gameObject.SetActive(true);
        }

        for (int i = remainingLives; i < maxHearts; i++)
        {
            Image heart = Instantiate(heartPrefab, heartsContainer);
            heart.gameObject.SetActive(false);
        }
    }
}
