using System.Collections.Generic;
using UnityEngine;

public class MobDraggable : MonoBehaviour
{
    //public GameObject indicatorPrefab; // Référence au préfab de la flèche
    //public Transform[] targets; // Les cibles vers lesquelles les flèches vont pointer
    //private List<GameObject> indicators = new List<GameObject>();

    bool drag;
    public Vector3 initPos = Vector3.zero;

    public bool isFromShop = true;
    public int index;

    public GameObject mobFxPrefab;

    private Rigidbody2D rb;
    private DroppableZone currentDroppableZone;

    private CharacterAnimatorController characterAnimatorController;

    public Animator animator;

    private GameObject[] droppableZones;
    private Vector3 offset = new(0, 0.5f, 0);

    public MouseHoverDetector mouseHoverDetector;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        UpdateDropTargets();

        droppableZones = GameObject.FindGameObjectsWithTag("DroppableZone");
        mouseHoverDetector = GetComponent<MouseHoverDetector>();

        // Trouvez le gestionnaire d'indicateurs dans la scène
        // indicatorManager = FindObjectOfType<IndicatorManager>();
        // if (indicatorManager == null)
        // {
        //     Debug.LogError("IndicatorManager not found in the scene.");
        // }
    }

    private void Update()
    {
        if (drag)
        {
            // TBD: Magic numbers for z-order to change
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
        animator.SetBool("IsWalking", true);
        initPos = transform.position;
        if (mouseHoverDetector != null)
            mouseHoverDetector.OnMouseDownCanvas();

        CreateAllIndicators();
    }

    private void OnMouseUp()
    {
        drag = false;
        animator.SetBool("IsWalking", false);
        DestroyAllIndicators();

        if (mouseHoverDetector != null)
            mouseHoverDetector.OnMouseUpCanvas();

        // Cancel the drag if the object is not dropped in a valid zone
        if (!currentDroppableZone || !currentDroppableZone.CanBeDropped())
        {
            Debug.Log("Drop in invalid zone");
            rb.MovePosition(initPos);
            return;
        }

        // Get the ID of the zone where the object is dropped
        int zoneId = currentDroppableZone.index;

        // Manage the case where the object is dropped at the same place
        if (!isFromShop && zoneId == index)
        {
            Debug.Log("Drop at the same place");
            rb.MovePosition(initPos);
            return;
        }

        // Manage the merge case
        if (VillageData.Instance.RoleAtIndex(zoneId) == gameObject.GetComponent<MobHealth>().mobData.role)
        {
            Debug.Log("Objects to merge");
            VillageData.Instance.FreeSpot(index);
            Destroy(gameObject);

            // Instantiate the FX for level up animation
            GameObject mobFx = Instantiate(mobFxPrefab, TeamManager.instance.GetMemberFromTeam(zoneId).transform.position, Quaternion.identity);
            // Set the sorting order of the FX to be on top of the mob
            int MobOrder = GetComponent<SpriteRenderer>().sortingOrder;
            mobFx.GetComponent<SpriteRenderer>().sortingOrder = MobOrder + 1;
            // Play level up animation don't need to destroy mobFx it destroy itself when animation is done
            mobFx.GetComponent<Animator>().SetTrigger("LevelUp");

            // Merge the objects
            return;
        }

        // Cancel the drag if the object is dropped in a zone that is not available
        if (zoneId > VillageData.Instance.Spots.Count || !VillageData.Instance.Spots[zoneId].IsAvailable)
        {
            Debug.Log("Drop in unavailable zone");
            rb.MovePosition(initPos);
            return;
        }

        // Manage the case where the object is dropped in a valid zone and come from the shop
        if (isFromShop)
        {
            isFromShop = false;
            // ContractActions.instance.TriggerHire((uint)index);
        }
        else
        {
            VillageData.Instance.FreeSpot(index);
        }

        Role role = gameObject.GetComponent<MobHealth>().mobData.role;
        VillageData.Instance.FillSpot(zoneId, role);
        rb.MovePosition(currentDroppableZone.transform.position + offset);

        // Update the team with new member
        TeamManager.instance.AddTeamMember(zoneId, gameObject);

        index = zoneId;
    }

    void UpdateDropTargets()
    {
        // Trouvez toutes les zones droppables dans la scène
        DroppableZone[] allDroppableZones = FindObjectsOfType<DroppableZone>();
        // Debug.Log("Found " + allDroppableZones.Length + " droppable zones.");

        // Utilisez une liste temporaire pour stocker les Transform des zones valides
        List<Transform> validDropTargets = new List<Transform>();

        foreach (DroppableZone zone in allDroppableZones)
        {
            if (VillageData.Instance.Spots[zone.index].IsAvailable)
            {
                validDropTargets.Add(zone.transform); // Ajoutez le Transform si la zone est valide
                // Debug.Log("Added a valid drop target: " + zone.transform.name);
            }
        }

        // Convertissez la liste en tableau et affectez-la à targets
        //if (indicatorManager != null)
        //indicatorManager.targets = validDropTargets.ToArray();
        // Debug.Log("Updated targets with " + targets.Length + " valid drop targets.");
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
