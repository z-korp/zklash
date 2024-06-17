using UnityEngine;

public class ProjectileLinear : MonoBehaviour
{
    public float speed = 5f;
    private Transform target;

    public void Initialize(Transform target)
    {
        this.target = target;
    }

    void Update()
    {
        if (target != null)
        {
            MoveTowardTarget();
            if (targetIsReach())
            {
                // Ajoutez des effets d'explosion ici
                Destroy(gameObject);
            }
        }
    }

    private void MoveTowardTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    private bool targetIsReach()
    {
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
            return true;
        return false;

    }
}
