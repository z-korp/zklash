using UnityEngine;
using System.Collections;

public class SheepAnimationController : MonoBehaviour
{
    private Animator animator;

    // Offset for each sheep in seconds
    [SerializeField] private float animationOffset = 0f;

    void Awake()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject.", this);
        }
    }

    void OnEnable()
    {
        // Start the animation with the offset when the GameObject is enabled
        if (animator != null)
        {
            StartCoroutine(StartAnimationWithOffset(animationOffset));
        }
    }

    void OnDisable()
    {
        // Stop all coroutines when the GameObject is disabled to avoid conflicts
        StopAllCoroutines();
    }

    private IEnumerator StartAnimationWithOffset(float offset)
    {
        // Wait for the specified delay before starting the animation
        yield return new WaitForSeconds(offset);

        // Set the Offset parameter in the animator to sync animations
        animator.SetFloat("Offset", offset);

        // Play the animation from the offset time
        animator.Play("sheepIdle", 0, offset);
    }
}