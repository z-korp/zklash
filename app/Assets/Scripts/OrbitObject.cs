using UnityEngine;

public class OrbitObject : MonoBehaviour
{
    public Transform target;
    public float orbitDistance = 0.5f;
    public float orbitSpeed = 130.0f;

    public Vector3 positionOffset = Vector3.zero;

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer targetSpriteRenderer;
    private float initialAngle = 0f; // Variable pour stocker l'angle initial

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (target != null)
        {
            targetSpriteRenderer = target.GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        if (target != null)
        {
            OrbitAroundTarget();
        }
    }

    void OrbitAroundTarget()
    {
        float angleRadians = (orbitSpeed * Time.time + initialAngle) * Mathf.Deg2Rad;
        Vector3 orbitPosition = new Vector3(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians), 0) * orbitDistance;
        transform.position = target.position + orbitPosition + positionOffset;

        // Handle the sprite sorting order
        if (transform.position.y >= target.position.y)
        {
            spriteRenderer.sortingOrder = targetSpriteRenderer.sortingOrder - 1;
        }
        else
        {
            spriteRenderer.sortingOrder = targetSpriteRenderer.sortingOrder + 1;
        }
    }

    public void SetInitialAngle(float angle)
    {
        initialAngle = angle;
    }
}
