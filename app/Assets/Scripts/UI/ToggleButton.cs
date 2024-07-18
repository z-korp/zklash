using UnityEngine;

public class ToggleButton : ButtonBase
{
    private bool isPressed = false;

    protected override void HandleButtonClick()
    {
        isPressed = !isPressed;
        image.sprite = isPressed ? pressedSprite : defaultSprite;
    }

    public override void OnButtonAction()
    {
        Debug.Log("Fais ce qu'il te plait bro toggle btn");
    }
}
