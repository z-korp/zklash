using UnityEngine;

public class StoneSizeController : MonoBehaviour
{
    public Sprite smallStone;
    public Sprite mediumStone;
    public Sprite largeStone;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void SetStoneSize(char size)
    {
        switch (size)
        {
            case 'S':
                SetSprite(smallStone);
                break;
            case 'M':
                SetSprite(mediumStone);
                break;
            case 'L':
                SetSprite(largeStone);
                break;
            default:
                Debug.LogWarning("Invalid size specified. Using default sprite.");
                break;
        }
    }

    private void SetSprite(Sprite newSprite)
    {
        if (spriteRenderer != null && newSprite != null)
        {
            spriteRenderer.sprite = newSprite;

            // Adjust collider size if you're using a BoxCollider2D
            if (TryGetComponent(out BoxCollider2D boxCollider))
            {
                boxCollider.size = newSprite.bounds.size;
            }

            // Reset the animation
            if (animator != null)
            {
                animator.Rebind();
                animator.Update(0f);
            }
        }
        else
        {
            Debug.LogError("SpriteRenderer or Sprite is null!");
        }
    }

    public void TriggerExplode()
    {
        animator.SetTrigger("explode");
    }
}