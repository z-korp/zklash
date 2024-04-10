using System.Collections;
using UnityEngine;


public class KnightAudio : MonoBehaviour
{

    public AudioClip attackKnightSound;

    private void Awake()
    {
    }

    void Start()
    {
    }

    void Update()
    {
    }

    public void PlayAttackKnightSound()
    {
        if (attackKnightSound != null)
            AudioManager.instance.PlayClipAt(attackKnightSound, transform.position);
    }

}
