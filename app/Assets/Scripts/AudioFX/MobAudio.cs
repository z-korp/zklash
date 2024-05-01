using System.Collections;
using UnityEngine;


public class MobAudio : MonoBehaviour
{

    public AudioClip attackSound;
    public AudioClip deathSound;

    private void Awake()
    {
    }

    void Start()
    {
    }

    void Update()
    {
    }

    public void PlayAttackSound()
    {
        if (attackSound != null)
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayClipAt(attackSound, transform.position);
            }
        }

    }

    public void PlayDeathSound()
    {
        if (deathSound != null)
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayClipAt(deathSound, transform.position);
            }
        }

    }

}
