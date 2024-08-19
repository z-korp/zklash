using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public abstract class ButtonBase : MonoBehaviour
{
    [SerializeField] protected Button button;
    [SerializeField] protected Image image;
    public Sprite defaultSprite;
    public Sprite pressedSprite;
    public Material grayscaleMaterial;

    private Material _defaultMaterial;
    protected Material DefaultMaterial
    {
        get
        {
            if (_defaultMaterial == null)
            {
                _defaultMaterial = new Material(Shader.Find("UI/Default"));
            }
            return _defaultMaterial;
        }
    }

    [SerializeField]
    private bool _isDisabled;
    public bool IsDisabled
    {
        get { return _isDisabled; }
        set
        {
            if (_isDisabled != value)
            {
                _isDisabled = value;
                UpdateButtonState();
            }
        }
    }

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
        UpdateButtonState();
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
        if (!IsDisabled)
        {
            HandleButtonClick();
            OnButtonAction();
        }
    }

    protected virtual void HandleButtonClick()
    {
        StartCoroutine(ButtonPressAnimation());
    }

    private IEnumerator ButtonPressAnimation()
    {
        if (pressedSprite != null && !IsDisabled)
        {
            image.sprite = pressedSprite;
            yield return new WaitForSeconds(0.1f);
            image.sprite = defaultSprite;
        }
    }

    private void UpdateButtonState()
    {
        if (button != null)
        {
            button.interactable = !IsDisabled;
        }

        if (image != null)
        {
            image.material = IsDisabled ? grayscaleMaterial : DefaultMaterial;
        }
    }

    public abstract void OnButtonAction();
}