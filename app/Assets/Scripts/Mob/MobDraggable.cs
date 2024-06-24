using System.Collections.Generic;
using UnityEngine;
using zKlash.Game.Roles;

public class MobDraggable : MonoBehaviour
{
    //public GameObject indicatorPrefab; // Référence au préfab de la flèche
    //public Transform[] targets; // Les cibles vers lesquelles les flèches vont pointer
    //private List<GameObject> indicators = new List<GameObject>();

    public Vector3 initPos = Vector3.zero;

    public bool isFromShop = true;
    public int index; // Index of the mob either in the shop (0, 1, 2) or in the team (0, 1, 2, 3)

    public GameObject mobFxPrefab;

    private Rigidbody2D _rb;
    private SpriteRenderer _sp;

    private bool _drag;

    private int _maxOrderValue = 1000;
    private DroppableZone currentDroppableZone;

    public Animator animator;

    private GameObject[] droppableZones;
    private Vector3 offset = new(0, 0.5f, 0);

    private bool togglingButton = false;

    public MouseHoverDetector mouseHoverDetector;

    private const int maxLevel = 3;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sp = GetComponent<SpriteRenderer>();
        UpdateDropTargets();

        droppableZones = GameObject.FindGameObjectsWithTag("DroppableZone");
        mouseHoverDetector = GetComponent<MouseHoverDetector>();
    }

    private void Update()
    {
        if (_drag)
        {
            // TBD: Magic numbers for z-order to change
            SetMobOrderVisual(_maxOrderValue);

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _rb.MovePosition(mousePos);

            if (!isFromShop && !togglingButton)
            {
                togglingButton = true;
                CanvasManager.instance.ToggleSellRerollButton();
            }
        }
        else
        {
            if (togglingButton)
            {
                togglingButton = false;
                CanvasManager.instance.ToggleSellRerollButton();
            }
            SetMobOrderVisual(_maxOrderValue - 1);
        }
    }

    private void OnMouseDown()
    {
        _drag = true;
        animator.SetBool("IsWalking", true);
        initPos = transform.position;
        if (mouseHoverDetector != null)
            mouseHoverDetector.OnMouseDownCanvas();

        CreateAllIndicators();
    }

    private void OnMouseUp()
    {
        _drag = false;
        animator.SetBool("IsWalking", false);
        DestroyAllIndicators();

        if (mouseHoverDetector != null)
            mouseHoverDetector.OnMouseUpCanvas();

        //TBD: SELL MOB, better solution ?
        // Check need to be done because we hide and show btn so the instance may not exist
        if (ClickButtonSell.instance != null)
            if (ClickButtonSell.instance.isDraggingSellMob)
                ClickButtonSell.instance.SellMob(gameObject);

        // Cancel the drag if the mob is not dropped in a valid zone
        if (InvalidDropZoneResetPosition()) return;

        // Get the index of the zone where the mob is dropped
        int zoneIndex = currentDroppableZone.index;

        // Manage the case where the mob is dropped at the same place when it doesn't come from the shop
        if (MobNotFromShopDropAtSamePlace(zoneIndex)) return;

        uint teamId = PlayerData.Instance.GetTeamId();

        // Manage the merge or swap case
        MergeOrSwapMob(zoneIndex, teamId);

    }

    private void SetMobOrderVisual(int spOrder)
    {
        _sp.sortingOrder = spOrder;
    }

    private void ResetPosition()
    {
        _rb.MovePosition(initPos);
    }

    private bool InvalidDropZoneResetPosition()
    {
        if (!currentDroppableZone || !currentDroppableZone.CanBeDropped())
        {
            Debug.Log("Drop in invalid zone");
            ResetPosition();
            return true;
        }

        return false;
    }

    private bool MobNotFromShopDropAtSamePlace(int zoneIndex)
    {
        if (!isFromShop && zoneIndex == index)
        {
            Debug.Log("Drop at the same place");
            ResetPosition();
            return true;
        }

        return false;
    }


    private void MergeOrSwapMob(int zoneIndex, uint teamId)
    {
        if (TeamManager.instance.RoleAtIndex(zoneIndex) == gameObject.GetComponent<MobController>().Character.Role.GetRole)
        {
            if (NotEnoughMoney()) return;

            GameObject mobToUpdate = TeamManager.instance.GetMemberFromTeam(zoneIndex);
            GameObject mobToRemove = gameObject;

            int oldLevel = mobToUpdate.GetComponent<MobController>().Character.Level;

            if (MobIsMaxLevel(zoneIndex, mobToUpdate, mobToRemove)) return;

            // Merge the mob
            MergeMobThatCanLevelUpUI(mobToUpdate, mobToRemove, oldLevel);

            if (isFromShop)
            {
                if (!MergeMobFromShopContract(mobToUpdate, teamId)) return;

            }
            else
            {
                if (!MergeMobNotFromShopContract(mobToUpdate, teamId)) return;

            }
        }
        else
        {
            // Cancel the drag if the mob is dropped in a zone that is not available
            if (zoneIndex > TeamManager.instance.TeamSpots.Length || !TeamManager.instance.TeamSpots[zoneIndex].IsAvailable)
            {

                // Spot is already taken in teamManager but don't come from shop that's mean
                // we want to invert both pos when we drag mob on top of another mob
                if (!isFromShop)
                {
                    // Swap function
                    swapUnit(index, zoneIndex);
                    index = zoneIndex;
                    return;
                }
                else
                {
                    Debug.Log("Drop in unavailable zone");
                    ResetPosition();
                    return;
                }

            }


            // Manage the case where the mob is dropped in a valid zone and come from the shop
            if (isFromShop)
            {
                if (PlayerData.Instance.purchaseCost > PlayerData.Instance.Gold)
                {
                    NoMoneyMessageResetPosition();
                    return;
                }

                isFromShop = false;
                StartCoroutine(TxCoroutines.Instance.ExecuteHire(teamId, (uint)index));
                CreateMobForTeam(zoneIndex);
                CanvasManager.instance.ToggleCanvasForDuration(2.0f);
            }
            else
            {
                SwapMobPositionInFreeSpotTeam(zoneIndex);
            }

            index = zoneIndex;
        }

    }

    private void NoMoneyMessageResetPosition()
    {
        DialogueManager.Instance.ShowDialogueForDuration("You're broke mate !", 2f);
        Debug.LogWarning("Not enough balance");
        ResetPosition();
    }

    private bool NotEnoughMoney()
    {
        if (isFromShop && PlayerData.Instance.purchaseCost > PlayerData.Instance.Gold)
        {
            NoMoneyMessageResetPosition();
            return true;
        }
        return false;
    }

    private bool MobIsMaxLevel(int zoneIndex, GameObject mobToUpdate, GameObject mobToRemove)
    {
        if (mobToUpdate.GetComponent<MobController>().Character.IsMobMaxLevel() || mobToRemove.GetComponent<MobController>().Character.IsMobMaxLevel())
        {
            if (isFromShop)
            {
                Debug.LogWarning("Mob is already Max Level");
                ResetPosition();
                return true;
            }

            // Mob is max level and we are in deck so you probably want to swap position
            swapUnit(index, zoneIndex);
            index = zoneIndex;
            return true;
        }

        return false;
    }

    private void swapUnit(int index, int zoneIndex)
    {
        // Get data from drag mob to populate teamManager spot
        string entity1 = TeamManager.instance.TeamSpots[index].Entity;
        Role role1 = gameObject.GetComponent<MobController>().Character.Role.GetRole;

        // Get data from standing mob to populate teamManager spot
        GameObject mob2 = TeamManager.instance.GetMemberFromTeam(zoneIndex);
        Role role2 = mob2.GetComponent<MobController>().Character.Role.GetRole;
        string entity2 = TeamManager.instance.TeamSpots[zoneIndex].Entity;

        // Free spots so we can fill them with new data
        TeamManager.instance.FreeSpot(index);
        TeamManager.instance.FreeSpot(zoneIndex);

        // Update spots in teamManager 
        TeamManager.instance.FillSpot(zoneIndex, role1, gameObject, entity1);
        TeamManager.instance.FillSpot(index, role2, mob2, entity2);

        // Update UI pos of entity
        _rb.MovePosition(currentDroppableZone.transform.position + offset);
        mob2.GetComponent<Rigidbody2D>().MovePosition(initPos);

        // Update index of the spot they are in battle deck
        mob2.GetComponent<MobDraggable>().index = index;
    }

    private void MergeMobThatCanLevelUpUI(GameObject mobToUpdate, GameObject mobToRemove, int oldLevel)
    {
        mobToUpdate.GetComponent<MobController>().Character.Merge(mobToRemove.GetComponent<MobController>().Character);
        Destroy(mobToRemove);

        int newLevel = mobToUpdate.GetComponent<MobController>().Character.Level;

        if (oldLevel != newLevel)
            LevelUpAnimation(mobToUpdate);
    }

    private bool MergeMobFromShopContract(GameObject mobToUpdate, uint teamId)
    {
        string entity = TeamManager.instance.GetEntityFromTeam(mobToUpdate);
        if (entity == "")
            return false;
        Character character = GameManager.Instance.worldManager.Entity(entity).GetComponent<Character>();
        StartCoroutine(TxCoroutines.Instance.ExecuteMergeFromShop(teamId, character.id, (uint)index));
        CanvasManager.instance.ToggleCanvasForDuration(2.0f);
        return true;
    }

    private bool MergeMobNotFromShopContract(GameObject mobToUpdate, uint teamId)
    {
        string fromEntity = TeamManager.instance.GetEntityFromTeam(mobToUpdate);
        if (fromEntity == "")
            return false;

        Character from = GameManager.Instance.worldManager.Entity(fromEntity).GetComponent<Character>();

        string toEntity = TeamManager.instance.GetEntityFromTeam(mobToUpdate);
        if (toEntity == "")
            return false;
        Character to = GameManager.Instance.worldManager.Entity(toEntity).GetComponent<Character>();

        StartCoroutine(TxCoroutines.Instance.ExecuteMerge(teamId, from.id, to.id));
        CanvasManager.instance.ToggleCanvasForDuration(2.0f);

        // Reset Team spot after merge
        TeamManager.instance.FreeSpot(index);

        // Reset sell btn to reroll
        CanvasManager.instance.ToggleSellRerollButton();

        return true;
    }

    private void CreateMobForTeam(int zoneIndex)
    {
        Role role = gameObject.GetComponent<MobController>().Character.Role.GetRole;
        TeamManager.instance.FillSpot(zoneIndex, role, gameObject);
        _rb.MovePosition(currentDroppableZone.transform.position + offset);
    }

    private void SwapMobPositionInFreeSpotTeam(int zoneIndex)
    {
        string entity = TeamManager.instance.TeamSpots[index].Entity;
        Role role = gameObject.GetComponent<MobController>().Character.Role.GetRole;
        TeamManager.instance.FillSpot(zoneIndex, role, gameObject, entity);
        _rb.MovePosition(currentDroppableZone.transform.position + offset);
        TeamManager.instance.FreeSpot(index);
    }

    private void LevelUpAnimation(GameObject mob)
    {
        // Instantiate the FX for level up animation
        GameObject mobFx = Instantiate(mobFxPrefab, mob.transform.position, Quaternion.identity);
        // Set the sorting order of the FX to be on top of the mob
        int MobOrder = GetComponent<SpriteRenderer>().sortingOrder;
        mobFx.GetComponent<SpriteRenderer>().sortingOrder = MobOrder + 1;
        // Play level up animation don't need to destroy mobFx it destroy itself when animation is done
        mobFx.GetComponent<Animator>().SetTrigger("LevelUp");
    }

    void UpdateDropTargets()
    {
        DroppableZone[] allDroppableZones = FindObjectsOfType<DroppableZone>();

        // Use this list to store the valid drop targets
        List<Transform> validDropTargets = new List<Transform>();

        foreach (DroppableZone zone in allDroppableZones)
        {
            if (TeamManager.instance.TeamSpots[zone.index].IsAvailable)
            {
                // Add transform of the zone if it is available
                validDropTargets.Add(zone.transform);
            }
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

    // Indicator methods
    private void DestroyAllIndicators()
    {
        if (droppableZones != null && droppableZones.Length > 0)
        {
            foreach (GameObject zone in droppableZones)
            {
                IndicatorManager indicatorManager = zone.GetComponent<IndicatorManager>();
                if (indicatorManager != null)
                {
                    indicatorManager.DestroyIndicator();
                }
            }
        }
    }

    private void CreateAllIndicators()
    {
        if (droppableZones != null && droppableZones.Length > 0)
        {
            foreach (GameObject zone in droppableZones)
            {
                IndicatorManager indicatorManager = zone.GetComponent<IndicatorManager>();
                if (indicatorManager != null)
                {
                    indicatorManager.CreateIndicator();
                }
            }
        }
    }

}
