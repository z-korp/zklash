using System.Collections;
using UnityEngine;

public class MobFx : MonoBehaviour
{
    public Animator levelUpAnimator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LevelUp()
    {
        levelUpAnimator.SetTrigger("LevelUp");
    }

    public void DeactivateMobFx()
    {
        gameObject.SetActive(false);
    }
    public void DestroyMobFx()
    {
        Destroy(gameObject, 0.1f);
    }

}
