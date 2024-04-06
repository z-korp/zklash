using UnityEngine;


public class Draggable : MonoBehaviour
{
    bool drag;
    public Vector3 initPos = Vector3.zero;

    private Rigidbody2D rb;
    private DroppableZone currentDroppableZone;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (drag)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.MovePosition(mousePos);
        }
    }

    private void OnMouseDown()
    {
        drag = true;
        initPos = transform.position;
    }

    private void OnMouseUp()
    {
        drag = false;
        if (currentDroppableZone != null && currentDroppableZone.CanBeDropped())
        {
            Debug.Log("Objet déposé dans la zone droppable.");
            ContractActions.instance.TriggerHire(0);
        }
        else
        {
            rb.MovePosition(initPos);
            Debug.Log("Objet non déposé.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DroppableZone zone = collision.GetComponent<DroppableZone>();
        if (zone != null)
        {
            currentDroppableZone = zone;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        DroppableZone zone = collision.GetComponent<DroppableZone>();
        if (zone == currentDroppableZone)
        {
            currentDroppableZone = null;
        }
    }

}
