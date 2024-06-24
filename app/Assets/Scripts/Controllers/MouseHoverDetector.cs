using UnityEngine;

public class MouseHoverDetector : MonoBehaviour
{
    public GameObject canvas;

    private bool isDragging = false;

    void Start()
    {
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
    }


    void OnMouseEnter()
    {
        if (canvas != null && !isDragging)
        {
            canvas.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if (canvas != null && !isDragging)
        {
            canvas.SetActive(false);
        }
    }

    public void OnMouseDownCanvas()
    {
        if (canvas != null)
        {
            isDragging = true;
            canvas.SetActive(false);
        }
    }

    public void OnMouseUpCanvas()
    {
        if (canvas != null)
        {
            isDragging = false;
            canvas.SetActive(true);
        }
    }

}
