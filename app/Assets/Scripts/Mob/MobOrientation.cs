using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Orientation
{
    Right,
    Left,
}

public class MobOrientation : MonoBehaviour
{
    public Orientation orientation;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on the GameObject.", this);
        }
        else
        {
            SetOrientation(orientation);
        }
    }

    public void SetOrientation(Orientation newOrientation)
    {
        if (spriteRenderer != null)
        {
            orientation = newOrientation;
            Debug.Log($"Setting orientation to: {orientation}", this);

            // Flip the sprite based on the orientation
            spriteRenderer.flipX = (orientation == Orientation.Left);

            Debug.Log($"SpriteRenderer.flipX is now: {spriteRenderer.flipX}", this);
        }
        else
        {
            Debug.LogError("SpriteRenderer component not found on the GameObject.", this);
        }
    }

    void Update()
    {
        // Check if the orientation is consistent
        if (spriteRenderer != null)
        {
            bool expectedFlipX = (orientation == Orientation.Left);
            if (spriteRenderer.flipX != expectedFlipX)
            {
                Debug.LogWarning($"SpriteRenderer.flipX inconsistent. Expected: {expectedFlipX}, Actual: {spriteRenderer.flipX}", this);
                SetOrientation(orientation);
            }
        }
    }
}