using UnityEngine;

public class DroppableZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Tuile déplaçable entrée dans la zone de dépôt");
        }

        Debug.Log("Tuile déplaçable entrée dans la zone de dépôt 2");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Draggable draggable = collision.gameObject.GetComponent<Draggable>();

        // Vérifiez si l'objet sortant est une tuile déplaçable
        if (draggable != null)
        {
            Debug.Log("Tuile déplaçable sortie de la zone de dépôt");
            // Vous pouvez ajouter ici la logique pour gérer la tuile déplaçable lorsqu'elle quitte la zone
        }
    }
}
