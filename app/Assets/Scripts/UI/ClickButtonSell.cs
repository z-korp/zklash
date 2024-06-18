using UnityEngine;
using UnityEngine.EventSystems;

public class ClickButtonSell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static ClickButtonSell instance;

    public bool isDraggingSellMob = false;

    public GameObject imageCoin;


    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one ClickButtonSell in the scene!");
            return;
        }
        instance = this;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        isDraggingSellMob = true;
        Debug.Log("Sell Mob?");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isDraggingSellMob = false;
        Debug.Log("En fait non!");
    }

    public void SellMob(GameObject draggedObject)
    {
        Debug.Log("Selling mob: ------------------>");
        Debug.Log("Selling mob: " + draggedObject.name);
        uint teamId = PlayerData.Instance.GetTeamId();
        string entity = TeamManager.instance.GetEntityFromTeam(draggedObject);
        if (entity == "")
        {
            Debug.Log("Entity not found.");
            return;
        }
        Character character = GameManager.Instance.worldManager.Entity(entity).GetComponent<Character>();
        StartCoroutine(TxCoroutines.Instance.ExecuteSell(teamId, character.id));

        // Get index to free spot in team and destroy gameObject
        int index = draggedObject.GetComponent<MobDraggable>().index;
        Destroy(draggedObject);
        TeamManager.instance.FreeSpot(index);
        CanvasManager.instance.ToggleSellRerollButton();
        isDraggingSellMob = false;
        imageCoin.SetActive(true);
        imageCoin.GetComponent<Animator>().SetTrigger("makeCoinPop");

    }
}
