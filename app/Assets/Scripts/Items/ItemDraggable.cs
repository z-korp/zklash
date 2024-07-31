using System;
using UnityEngine;
using zKlash.Game.Items;

public class ItemDraggable : MonoBehaviour
{
    bool drag;
    public Vector3 initPos = Vector3.zero;
    public bool isFromShop = true;
    public int index;
    public ItemData item;
    public IItem Item;
    private Rigidbody2D rb;
    public bool canDropItem;

    public GameObject orbitObjectPrefab;

    private GameObject mob;

    public delegate void ItemHoveredHandler(bool isHovered);
    public event ItemHoveredHandler OnItemHovered;

    private DialogueManager _dialogueManager;
    private TeamManager _teamManager;
    private ShopMessageManager _shopMessageManager;

    private string _priceString = "It will cost you: \n\n";

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _dialogueManager = DialogueManager.Instance;
        _teamManager = TeamManager.Instance;
        _shopMessageManager = ShopMessageManager.Instance;

        Item = ItemFactory.GetItem(item.type);
    }

    void Update()
    {
        if (drag)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 1000;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.MovePosition(mousePos);

            // On Drag hide HUD
            OnItemHovered?.Invoke(false);
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
            if (PlayerData.Instance.Gold < Item.Cost())
            {
                _dialogueManager.ShowDialogueForDuration("You're broke mate !", 2f);
                Debug.LogWarning("Not enough gold to purchase item.");
                rb.MovePosition(initPos);
                return;
            }

            // Smart contract call
            string entity = _teamManager.GetEntityFromTeam(mob);
            if (entity == "")
            {
                Debug.Log("Entity not found.");
                rb.MovePosition(initPos);
                return;
            }
            Character character = GameManager.Instance.worldManager.Entity(entity).GetComponent<Character>();
            uint teamId = PlayerData.Instance.GetTeamId();
            StartCoroutine(TxCoroutines.Instance.ExecuteEquip(
                teamId,
                character.id,
                (uint)index,
                () =>
                {
                    Debug.Log("=> Item equipped on mob: " + mob.name + " with item: " + item.name);

                    // On success equip item
                    isFromShop = false;
                    mob.GetComponent<MobItem>().item = item;
                    Debug.Log("Item dropped on mob: " + mob.name + " with item: " + item.name);
                    mob.GetComponent<MobController>().Character.Equip(item.type);

                    Destroy(gameObject);
                },
                (error) =>
                {
                    // On error move item back to initial position
                    Debug.LogError("=> Error in ExecuteEquip: " + error);
                    _dialogueManager.ShowDialogueForDuration("Error during equip", 2f);
                    rb.MovePosition(initPos);
                }
            ));
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

            mob = collision.gameObject;
            var mobFromShop = mob.GetComponent<MobDraggable>().isFromShop;
            if (!mobFromShop)
            {
                canDropItem = true;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canDropItem = false;
    }

    private void OnMouseEnter()
    {
        OnItemHovered?.Invoke(true);
        _shopMessageManager.ShowShopMessage(_priceString + Item.Cost().ToString());

    }
    private void OnMouseExit()
    {
        OnItemHovered?.Invoke(false);
        _shopMessageManager.ResetShopMessage();
    }
}
