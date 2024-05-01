using UnityEngine;

public class ItemDraggable : MonoBehaviour
{
    bool drag;
    public Vector3 initPos = Vector3.zero;
    public bool isFromShop = true;
    public int index;
    public ItemData item;
    private Rigidbody2D rb;
    public bool canDropItem;

    public GameObject orbitObjectPrefab;

    private GameObject mob;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
    }

    void Update()
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
            //GameObject itemOrbiterGO = Instantiate(orbitObjectPrefab, mob.transform.position, Quaternion.identity);
            //OrbitObject itemOrbiter = itemOrbiterGO.GetComponent<OrbitObject>();
            //itemOrbiter.target = mob.transform;
            //itemOrbiterGO.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            mob.GetComponent<MobItem>().item = item;
            mob.GetComponent<MobController>().Character.Equip(item.type);

            // Smart contract call
            string entity = TeamManager.instance.GetEntityFromTeam(mob);
            if (entity == "")
            {
                Debug.Log("Entity not found.");
                return;
            }
            Character character = GameManager.Instance.worldManager.Entity(entity).GetComponent<Character>();
            ContractActions.instance.TriggerEquip(character.id, (uint)index);

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Object not dropped.");
            rb.MovePosition(initPos);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mob"))
        {
            Debug.Log("Mob detected");
            canDropItem = true;
            mob = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canDropItem = false;
    }
}
