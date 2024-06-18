using UnityEngine;

public class SheepRotation : MonoBehaviour
{
    public Transform centerPoint; // Le point autour duquel le mouton va tourner (la tête du mob)
    public float rotationSpeed = 100f; // La vitesse de rotation

    void Update()
    {
        if (centerPoint != null)
        {
            // Faire tourner le mouton autour du point central
            transform.RotateAround(centerPoint.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }
}
