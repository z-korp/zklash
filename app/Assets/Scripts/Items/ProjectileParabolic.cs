using UnityEngine;

public class ProjectileParabolic : MonoBehaviour
{
    private Transform target;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float speed;
    private float arcHeight;
    private float timeToTarget;
    private float elapsedTime;

    private bool hitted = false;

    private Animator _animator;



    public void Initialize(Transform target, float speed = 3f, float arcHeight = 2f)
    {
        this.target = target;
        this.startPosition = transform.position;
        this.targetPosition = target.position;
        this.speed = speed;
        this.arcHeight = arcHeight;

        // Calculate the time to reach the target
        float distance = Vector3.Distance(startPosition, targetPosition);
        this.timeToTarget = distance / speed;
        this.elapsedTime = 0f;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (target != null && !hitted)
        {
            MoveTowardTargetCurve();
        }
    }

    private void MoveTowardTargetCurve()
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / timeToTarget;

        // Calculate the new position
        Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, t);

        // Add the arc height
        currentPosition.y += arcHeight * Mathf.Sin(Mathf.PI * t);

        // Update the projectile position
        transform.position = currentPosition;

        // Check if the projectile has reached the target
        if (t >= 1f)
        {
            OnProjectileHit();
        }
    }

    private void OnProjectileHit()
    {
        hitted = true;
        // Here you can add logic for when the projectile hits the target, such as applying damage.
        _animator.SetTrigger("explode");
        //Destroy(gameObject);
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
