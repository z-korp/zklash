using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Vector2 hotSpot = Vector2.zero;

    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }
}