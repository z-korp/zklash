using System.Collections;
using UnityEngine;


public class TorchoblinAudio : MonoBehaviour
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

    public void playAttackSound()
    {
        AudioManager.instance.PlayClipAt(attackSound, transform.position);
    }

}
