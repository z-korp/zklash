using UnityEngine;

public class ItemDraggable : MonoBehaviour
{
    bool drag;
    public Vector3 initPos = Vector3.zero;
    public bool isFromShop = true;
    public int index;
    private Rigidbody2D rb;
    public bool canDropItem;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (drag)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 1000;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.MovePosition(mousePos);
            // UpdateArrows();
        }
        else
        {
            GetComponent<SpriteRenderer>().sortingOrder = 999;
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

        if (canDropItem)
        {
            isFromShop = false;
        }
        else
        {
            gameObject.GetComponent<MobItem>().itemPrefab = gameObject;
            Debug.Log("Object not dropped.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Mob"))
        {
            Debug.Log("Mob detected");
            canDropItem = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canDropItem = false;
    }

}
