using System.Collections;
using UnityEngine;


public class ArcherAudio : MonoBehaviour
{

    public AudioClip attackArcherSound;

    private void Awake()
    {
    }

    void Start()
    {
    }

    void Update()
    {
    }

    public void PlayAttackArcherSound()
    {
        if (attackArcherSound != null)
        {
            AudioManager.instance.PlayClipAt(attackArcherSound, transform.position);
        }

    }

}
