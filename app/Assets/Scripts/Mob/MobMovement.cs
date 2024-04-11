using System.Collections.Generic;
using UnityEngine;

public class MobMovement : MonoBehaviour
{
    public int speed;
    private readonly List<Transform> targets = new();
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private readonly float epsilon = 0.05f;

    public void Update()
    {
        // If the target transform position is not close to the current position then
        // Translate from the current transform position to the target transform position at a specified speed
        // Aplly an easing effect to the movement In and Out
        // Then clear the target
        if (targets.Count != 0)
        {
            Transform target = targets[0];
            Vector3 direction = target.position - transform.position;
            Flip(direction.x);
            if (Mathf.Abs(direction.x) > epsilon)
            {
                animator.SetBool("IsWalking", true);
                // rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, Time.deltaTime);
                transform.Translate(speed * Time.deltaTime * direction.normalized, Space.World);
            }
            else
            {
                animator.SetBool("IsWalking", false);
                rb.velocity = Vector3.zero;
                targets.RemoveAt(0);
            }
        }
    }
    
    public void Move(Transform _target)
    {
        targets.Add(_target);
    }

    void Flip(float _movement)
    {
        if (_movement > epsilon)
        {
            spriteRenderer.flipX = false;
        }else if(_movement < -epsilon)
        {
            spriteRenderer.flipX = true;
        }
    }
}
