using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class ButtonBase : MonoBehaviour
{
    [SerializeField]
    protected Button button;
    [SerializeField]
    protected Image image;
    public Sprite defaultSprite;
    public Sprite pressedSprite;

    protected virtual void Awake()
    {
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    private void Start()
    {
        if (image != null)
        {
            image.sprite = defaultSprite;
        }
    }

    protected virtual void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClick);
        }
    }

    protected virtual void OnButtonClick()
    {
        HandleButtonClick();
        OnButtonAction();
    }

    protected virtual void HandleButtonClick()
    {
        StartCoroutine(ButtonPressAnimation());
    }

    private IEnumerator ButtonPressAnimation()
    {
        if (pressedSprite != null)
        {
            image.sprite = pressedSprite;
        }
        yield return new WaitForSeconds(0.1f);
        if (defaultSprite != null)
        {
            image.sprite = defaultSprite;
        }
    }

    public abstract void OnButtonAction();
}
