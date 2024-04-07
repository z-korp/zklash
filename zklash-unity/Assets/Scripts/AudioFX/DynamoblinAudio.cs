using System.Collections;
using UnityEngine;


public class DynamoblinAudio : MonoBehaviour
{

    public AudioClip attackDynamoSound;

    private void Awake()
    {
    }

    void Start()
    {
    }

    void Update()
    {
    }

    public void PlayAttackDynamoSound()
    {
        if (attackDynamoSound != null)
            AudioManager.instance.PlayClipAt(attackDynamoSound, transform.position);
    }

}
