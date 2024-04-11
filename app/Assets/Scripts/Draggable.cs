using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    //public GameObject indicatorPrefab; // Référence au préfab de la flèche
    //public Transform[] targets; // Les cibles vers lesquelles les flèches vont pointer
    //private List<GameObject> indicators = new List<GameObject>();

    bool drag;
    public Vector3 initPos = Vector3.zero;

    public bool isFromShop = true;

    private Rigidbody2D rb;
    private DroppableZone currentDroppableZone;

    public Animator animator;

    private GameObject[] droppableZones;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        UpdateDropTargets();

        droppableZones = GameObject.FindGameObjectsWithTag("DroppableZone");

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

        CreateAllIndicators();
    }

    private void OnMouseUp()
    {
        drag = false;
        animator.SetBool("IsWalking", false);

        DestroyAllIndicators();

        if (currentDroppableZone != null && currentDroppableZone.CanBeDropped())
        {
            string zoneName = currentDroppableZone.gameObject.name;
            string idString = zoneName.Split('_')[1]; // Split the name by '_' and take the second part
            int zoneId = int.Parse(idString); // Convert the ID part to an integer

            if (currentDroppableZone != null && currentDroppableZone.onZone == gameObject.GetComponent<ElementData>().mobData.title)
            {
                // TBD :: Fusionner les deux objets dans la zone
                Debug.Log("Objet à fusionner");

            }

            if (VillageData.Instance.Spots.Count > zoneId && VillageData.Instance.Spots[zoneId].IsAvailable)
            {
                // Debug.Log($"Objet déposé dans la zone droppable: {currentDroppableZone.gameObject.name} ${zoneId}.");

                if (isFromShop)
                {

                    ElementData data = gameObject.GetComponent<ElementData>();
                    ContractActions.instance.TriggerHire(data.indexFromShop);
                    data.index = zoneId;
                    isFromShop = false;
                    currentDroppableZone.onZone = gameObject.GetComponent<ElementData>().mobData.title;

                }

                VillageData.Instance.FillSpot(zoneId, null);
            }
            else
            {
                rb.MovePosition(initPos);
                // Debug.Log("Object not dropped. Zone not available.");
            }
        }
        else
        {
            rb.MovePosition(initPos);
            // Debug.Log("Object not dropped.");
        }
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
            // Debug.Log("Checking droppable zone: " + zone.name);
            string zoneName = zone.name;
            string idString = zoneName.Split('_')[1]; // Split the name by '_' and take the second part
            int zoneId = int.Parse(idString); // Convert the ID part to an integer
            // Debug.Log("Zone ID: " + zoneId);
            // Debug.Log("Is available: " + VillageData.Instance.Spots[zoneId].IsAvailable);
            if (VillageData.Instance.Spots[zoneId].IsAvailable)
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
