using System.Collections;
using UnityEngine;


public class BomboblinAudio : MonoBehaviour
{

    public AudioClip attackBombolinSound;

    private void Awake()
    {
    }

    void Start()
    {
    }

    void Update()
    {
    }

    public void PlayAttackBomboSound()
    {
        if (attackBombolinSound != null)
            AudioManager.instance.PlayClipAt(attackBombolinSound, transform.position);
    }

}
