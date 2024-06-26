using System.Collections.Generic;
using UnityEngine;

public class MobMovement : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private CharacterAnimatorController characterAnimatorController;

    public float speed;
    private readonly List<Transform> targets = new();


    private readonly float epsilon = 0.05f;

    void Awake()
    {
        animator = GetComponentInParent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found in parent GameObject.", this);
        }

        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found in parent GameObject.", this);
        }

        rb = GetComponentInParent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found in parent GameObject.", this);
        }

        characterAnimatorController = GetComponentInParent<CharacterAnimatorController>();
        if (characterAnimatorController == null)
        {
            Debug.LogError("CharacterAnimatorController component not found in parent GameObject.", this);
        }
    }

    void Update()
    {
        IsMoving();
    }

    public void Move(Transform _target)
    {
        targets.Add(_target);
    }



    public bool IsMoving()
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
                //animator.SetBool("IsWalking", true);
                // rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, Time.deltaTime);
                characterAnimatorController.Walk(true);
                transform.Translate(speed * Time.deltaTime * direction.normalized, Space.World);
                return true;
            }
            else
            {
                characterAnimatorController.Idle();
                //animator.SetBool("IsWalking", false);
                rb.velocity = Vector3.zero;
                targets.RemoveAt(0);
                return false;
            }
        }

        return false;
    }

    void Flip(float _movement)
    {
        if (_movement > epsilon)
        {
            spriteRenderer.flipX = false;
        }
        else if (_movement < -epsilon)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void ResetTarget()
    {
        targets.Clear();
    }
}
