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

    void OnMouseDown()
    {
        if (canvas != null)
        {
            isDragging = true;
            canvas.SetActive(false);
        }
    }

    void OnMouseUp()
    {
        if (canvas != null)
        {
            isDragging = false;
        }
    }

}
