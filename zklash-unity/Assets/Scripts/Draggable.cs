using UnityEngine;


public class Draggable : MonoBehaviour
{
    bool drag;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnMouseDown()
    {
        Debug.Log("Mouse down");
        drag = true;
    }

    private void OnMouseUp()
    {
        Debug.Log("Mouse up");
        drag = false;
    }

    private void Update()
    {
        if (drag)
        {
            Debug.Log("Dragging");
            //Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            //transform.Translate(MousePos);
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.MovePosition(mousePos);

        }
    }

}
