using System.Collections;
using UnityEngine;


public class TorchoblinAudio : MonoBehaviour
{

    public AudioClip attackTorchoblinSound;

    private void Awake()
    {
    }

    void Start()
    {
    }

    void Update()
    {
    }

    public void PlayAttackTorchoblinSound()
    {
        if (attackTorchoblinSound != null)
            AudioManager.instance.PlayClipAt(attackTorchoblinSound, transform.position);
    }

}
