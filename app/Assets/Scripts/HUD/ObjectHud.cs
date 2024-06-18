using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ObjectHud : MonoBehaviour
{

    public ItemDraggable item;
    private Image _image;
    private TxtItem _text;
    public ItemData itemData;

    private void Awake()
    {
        _image = GetComponentInChildren<Image>();
        _text = GetComponentInChildren<TxtItem>();
    }

    private void Start()
    {
        _image.enabled = false;
        InitText();

    }

    private void InitText()
    {
        if (_text == null)
        {
            Debug.LogError("Text is null");
        }
        else
        {
            _text.HideShowText(false);
            _text.UpdateText(itemData.description);
        }
    }

    private void OnEnable()
    {
        item.OnItemHovered += OnItemOveredChanged;
    }

    private void OnDisable()
    {
        item.OnItemHovered -= OnItemOveredChanged;
    }

    private void OnItemOveredChanged(bool isHovered)
    {
        if (_image != null)
        {
            _image.enabled = isHovered;
            _text.HideShowText(isHovered);
        }
        else Debug.LogError("Image is null");
    }
}
