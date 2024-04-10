using System.Collections;
using UnityEngine;


public class MobAudio : MonoBehaviour
{

    public AudioClip attackSound;

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
            AudioManager.instance.PlayClipAt(attackSound, transform.position);
        }

    }

}
