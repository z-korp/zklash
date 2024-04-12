using UnityEngine;

public class OrbitObject : MonoBehaviour
{
    public Transform target;
    public float orbitDistance = 0.5f;
    public float orbitSpeed = 130.0f;

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer targetSpriteRenderer;

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
        float angleRadians = orbitSpeed * Time.time * Mathf.Deg2Rad;
        transform.position = target.position + new Vector3(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians), 0) * orbitDistance;

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
}
