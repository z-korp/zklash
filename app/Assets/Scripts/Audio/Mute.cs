using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Mute : MonoBehaviour
{
    public Image _imageBtn;
    public Image _imageMute;
    public Sprite _defaultBtn, _pressedBtn;
    public Sprite _defaultMute, _pressedMute;

    private bool isMuted = false;

    private void Start()
    {
        UpdateMuteState(false);
    }

    public void MuteUnmuteVolume()
    {
        isMuted = !isMuted;
        UpdateMuteState(isMuted);
    }

    private void UpdateMuteState(bool muted)
    {
        isMuted = muted;

        if (isMuted)
        {
            _imageBtn.sprite = _pressedBtn;
            _imageMute.sprite = _pressedMute;
            AudioManager.Instance.DisableSound();
        }
        else
        {
            _imageBtn.sprite = _defaultBtn;
            _imageMute.sprite = _defaultMute;
            AudioManager.Instance.EnableSound();
        }
    }
}