using System.Collections;
using UnityEngine;


public class PawAudio : MonoBehaviour
{

    public AudioClip attackPawnSound;

    private void Awake()
    {
    }

    void Start()
    {
    }

    void Update()
    {
    }

    public void PlayAttackPawnSound()
    {
        if (attackPawnSound != null)
            AudioManager.instance.PlayClipAt(attackPawnSound, transform.position);
    }

}
