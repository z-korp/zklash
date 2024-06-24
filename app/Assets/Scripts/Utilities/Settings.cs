using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Data.Common;

public class Settings : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image _imageBtn;
    public Image _imageSettings;
    public Sprite _defaultBtn, _pressedBtn;
    public Sprite _defaultSettings, _pressedSettings;

    public void OnPointerDown(PointerEventData eventData)
    {
        _imageBtn.sprite = _pressedBtn;
        _imageSettings.sprite = _pressedSettings;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _imageBtn.sprite = _defaultBtn;
        _imageSettings.sprite = _defaultSettings;
    }

}

