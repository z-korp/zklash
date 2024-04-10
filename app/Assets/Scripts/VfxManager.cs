using UnityEngine;

public class VfxManager : MonoBehaviour
{
    public Animator animatorVFX;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.H))
        // {
        //     animatorVFX.SetTrigger("Explode");
        // }

    }

    public void PlayExplodeVFX()
    {
        animatorVFX.SetTrigger("Explode");
    }

    public void PlayStopVFX()
    {
        animatorVFX.SetTrigger("Stop");
    }
}
