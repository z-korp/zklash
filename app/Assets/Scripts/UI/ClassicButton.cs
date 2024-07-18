using UnityEngine;

public class ClassicButton : ButtonBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnButtonAction()
    {
        Debug.Log("Classic button clicked!");
    }
}
