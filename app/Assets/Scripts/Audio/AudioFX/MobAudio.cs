using System.Collections;
using UnityEngine;


public class MobAudio : MonoBehaviour
{

    public AudioClip attackSound;
    public AudioClip deathSound;

    private AudioManager _audioManager;

    private void Awake()
    {
    }

    void Start()
    {
        _audioManager = AudioManager.Instance;
    }

    void Update()
    {
    }

    public void PlayAttackSound()
    {
        if (attackSound != null)
        {
            if (_audioManager != null)
            {
                _audioManager.PlayClipAt(attackSound, transform.position);
            }
        }

    }

    public void PlayDeathSound()
    {
        if (deathSound != null)
        {
            if (_audioManager != null)
            {
                _audioManager.PlayClipAt(deathSound, transform.position);
            }
        }

    }

}
