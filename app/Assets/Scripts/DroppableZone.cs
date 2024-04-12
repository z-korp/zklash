using UnityEngine;

public class DroppableZone : MonoBehaviour
{
    public int index;
    private bool isDroppable;

    void Start()
    {
        index = DroppableManager.instance.IndexOf(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MobDraggable draggable = collision.gameObject.GetComponent<MobDraggable>();
        if (draggable != null)
        {
            Debug.Log("Player entered droppable zone");
            isDroppable = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MobDraggable draggable = collision.gameObject.GetComponent<MobDraggable>();

        if (draggable != null)
        {
            isDroppable = false;
        }
    }

    public bool CanBeDropped()
    {
        return isDroppable;
    }
}
