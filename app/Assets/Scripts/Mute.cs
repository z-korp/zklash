using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Data.Common;

public class Mute : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image _imageBtn;
    public Image _imageMute;
    public Sprite _defaultBtn, _pressedBtn;
    public Sprite _defaultMute, _pressedMute;
    public AudioMixer audioMixer;

    private bool isMuted = false;

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void MuteUnmuteVolume()
    {
        if (isMuted)
        {
            _imageBtn.sprite = _defaultBtn;
            _imageMute.sprite = _defaultMute;
            UnmuteAudio();
            isMuted = false;
        }
        else
        {
            _imageBtn.sprite = _pressedBtn;
            _imageMute.sprite = _pressedMute;
            MuteAudio();
            isMuted = true;
        }
    }

    public void MuteAudio()
    {
        AudioListener.pause = true;
    }

    public void UnmuteAudio()
    {
        AudioListener.pause = false;
    }


}

