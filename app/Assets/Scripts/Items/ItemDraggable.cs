using System.Collections.Generic;
using UnityEngine;

public class ItemDraggable : MonoBehaviour
{
    bool drag;
    public Vector3 initPos = Vector3.zero;
    public bool isFromShop = true;
    public int index;
    private Rigidbody2D rb;
    private DroppableZone currentDroppableZone;
    private GameObject[] droppableZones;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        UpdateDropTargets();
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

        // Créez les indicateurs lorsque le drag commence
        CreateAllIndicators();

    }

    private void OnMouseUp()
    {
        drag = false;

        // Détruisez les indicateurs lorsque le drag se termine
        DestroyAllIndicators();

        if (currentDroppableZone != null && currentDroppableZone.CanBeDropped())
        {
            isFromShop = false;
        }
        else
        {
            rb.MovePosition(initPos);
            Debug.Log("Object not dropped.");
        }
    }

    void UpdateDropTargets()
    {
        // Trouvez toutes les zones droppables dans la scène
        DroppableZone[] allDroppableZones = FindObjectsOfType<DroppableZone>();

        // Utilisez une liste temporaire pour stocker les Transform des zones valides
        List<Transform> validDropTargets = new List<Transform>();

        foreach (DroppableZone zone in allDroppableZones)
        {
            Debug.Log("Checking droppable zone: " + zone.name);
            if (VillageData.Instance.Spots[zone.index].IsAvailable)
            {
                validDropTargets.Add(zone.transform); // Ajoutez le Transform si la zone est valide
                Debug.Log("Added a valid drop target: " + zone.transform.name);
            }
        }

        // Convertissez la liste en tableau et affectez-la à targets
        // indicatorManager.targets = validDropTargets.ToArray();
        // Debug.Log("Updated targets with " + indicatorManager.targets.Length + " valid drop targets.");
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
