using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour
{
    public GameObject indicatorPrefab; // Référence au préfab de la flèche
    public Transform[] targets; // Les cibles vers lesquelles les flèches vont pointer

    private List<GameObject> indicators = new List<GameObject>();

    bool drag;
    public Vector3 initPos = Vector3.zero;

    public bool isFromShop = true;

    private Rigidbody2D rb;
    private DroppableZone currentDroppableZone;

    public Animator animator;


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
        animator.SetBool("IsWalking", true);
        initPos = transform.position;

        CreateIndicators();
    }

    private void OnMouseUp()
    {
        drag = false;
        animator.SetBool("IsWalking", false);
        DestroyIndicators();
        if (currentDroppableZone != null && currentDroppableZone.CanBeDropped())
        {
            string zoneName = currentDroppableZone.gameObject.name;
            string idString = zoneName.Split('_')[1]; // Split the name by '_' and take the second part
            int zoneId = int.Parse(idString); // Convert the ID part to an integer

            if (currentDroppableZone != null && currentDroppableZone.onZone == gameObject.GetComponent<ElementData>().mobData.nameMob)
            {
                // TBD :: Fusionner les deux objets dans la zone
                Debug.Log("Objet à fusionner");

            }

            if (VillageData.Instance.Spots.Count > zoneId && VillageData.Instance.Spots[zoneId].IsAvailable)
            {
                // Debug.Log($"Objet déposé dans la zone droppable: {currentDroppableZone.gameObject.name} ${zoneId}.");

                if (isFromShop)
                {

                    //ElementData data = gameObject.GetComponent<ElementData>();
                    //ContractActions.instance.TriggerHire(data.indexFromShop);
                    //data.index = zoneId;
                    isFromShop = false;
                    currentDroppableZone.onZone = gameObject.GetComponent<ElementData>().mobData.nameMob;

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
        targets = validDropTargets.ToArray();
        // Debug.Log("Updated targets with " + targets.Length + " valid drop targets.");
    }

    private void CreateIndicators()
    {
        foreach (Transform target in targets)
        {
            GameObject indicator = Instantiate(indicatorPrefab, target.position, Quaternion.identity);
            indicators.Add(indicator);
            StartCoroutine(AnimateIndicator(indicator.transform));
            // Debug.Log("Created indicator for target: " + target.name);
        }
    }

    private IEnumerator AnimateIndicator(Transform indicatorTransform)
    {
        // Valeurs pour l'animation de scale up et scale down
        float duration = 0.5f; // Durée d'un cycle
        Vector3 scaleUp = new Vector3(1.5f, 1.5f, 1f); // Taille maximale
        Vector3 scaleDown = new Vector3(1.2f, 1.2f, 1f); // Taille minimale

        while (indicatorTransform != null)
        {
            // Scale up
            float timer = 0f;
            while (timer <= duration)
            {
                indicatorTransform.localScale = Vector3.Lerp(indicatorTransform.localScale, scaleUp, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            // Scale down
            timer = 0f;
            while (timer <= duration)
            {
                indicatorTransform.localScale = Vector3.Lerp(indicatorTransform.localScale, scaleDown, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }

    private void DestroyIndicators()
    {
        foreach (GameObject indicator in indicators)
        {
            StopAllCoroutines(); // Arrête toutes les coroutines en cours pour cet objet
            Destroy(indicator);
        }
        indicators.Clear();
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
