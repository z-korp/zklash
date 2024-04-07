using UnityEngine;

public class MouseHoverDetector : MonoBehaviour
{
    public GameObject canvas;

    void Start()
    {
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
    }

   
    void OnMouseEnter()
    {
        if (canvas != null)
        {
            canvas.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
    }
}
