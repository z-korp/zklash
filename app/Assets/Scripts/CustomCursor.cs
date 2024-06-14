using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Vector2 hotSpot = new Vector2(23, 18);

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // This makes the object persistent across scenes
    }

    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }
}