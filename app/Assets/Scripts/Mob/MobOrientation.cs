using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobOrientation : MonoBehaviour
{
    public enum Orientation
    {
        Left,
        Right
    }

    public Orientation orientation;

    private SpriteRenderer spriteRenderer;

    void Start()
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

            // Flip the sprite based on the orientation
            spriteRenderer.flipX = (orientation == Orientation.Left);
        }
    }
}
