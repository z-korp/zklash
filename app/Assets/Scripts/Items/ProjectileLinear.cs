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
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                // Ajoutez des effets d'explosion ici
                Destroy(gameObject);
            }
        }
    }
}
