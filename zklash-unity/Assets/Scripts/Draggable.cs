using UnityEngine;


public class Draggable : MonoBehaviour
{
    bool drag;
    public Vector3 initPos = Vector3.zero;

    public bool isFromShop = true;

    private Rigidbody2D rb;
    private DroppableZone currentDroppableZone;

    public Animator animator;


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
        animator.SetBool("IsWalking", true);
        initPos = transform.position;

    }

    private void OnMouseUp()
    {
        drag = false;
        animator.SetBool("IsWalking", false);
        if (currentDroppableZone != null && currentDroppableZone.CanBeDropped())
        {
            string zoneName = currentDroppableZone.gameObject.name;
            string idString = zoneName.Split('_')[1]; // Split the name by '_' and take the second part
            int zoneId = int.Parse(idString); // Convert the ID part to an integer

            if (VillageData.Instance.Spots.Count > zoneId && VillageData.Instance.Spots[zoneId].IsAvailable)
            {
                Debug.Log($"Objet déposé dans la zone droppable: {currentDroppableZone.gameObject.name}.");

                if (isFromShop)
                {
                    ContractActions.instance.TriggerHire(0);
                    isFromShop = false;
                }

                VillageData.Instance.FillSpot(zoneId, gameObject.name);
            }
            else
            {
                rb.MovePosition(initPos);
                Debug.Log("Object not dropped. Zone not available.");
            }
        }
        else
        {
            rb.MovePosition(initPos);
            Debug.Log("Object not dropped.");
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
