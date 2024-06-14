using UnityEngine;

public class CharacterAnimatorController : MonoBehaviour
{
    Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Walk(bool doWalk)
    {
        animator.SetBool("IsWalking", doWalk);
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }


    public void Idle()
    {
        animator.SetBool("IsWalking", false);
        animator.ResetTrigger("Attack");
    }
}