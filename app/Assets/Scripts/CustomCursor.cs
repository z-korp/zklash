using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Vector2 hotSpot = new Vector2(23, 18);

    void Start()
    {
        Debug.Log("Cursor texture: " + cursorTexture.width);
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }
}