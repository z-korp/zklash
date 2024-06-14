using System.Collections.Generic;
using UnityEngine;

public class MobStatus : MonoBehaviour
{
    private MobController mobController;
    public GameObject stunEffectPrefab;

    public List<GameObject> stunAnimationList;

    public int stunDuration = 1;
    public int stun;
    public int absorb;

    public bool isPlaying = false;



    public void Start()
    {
        // Get the MobController component from the GameObject
        mobController = GetComponent<MobController>();

        if (mobController == null)
        {
            Debug.LogError("MobController component not found on the GameObject.", this);
            return;
        }

        UpdateStatusAnimation();
    }

    public void Update()
    {
        UpdateStatusAnimation();
    }

    private void UpdateStatusAnimation()
    {
        if (mobController != null && mobController.Character != null)
        {
            stun = mobController.Character.Stun;
            absorb = mobController.Character.Absorb;
        }

        if (stun != 0 && !isPlaying)
        {
            ApplyStunEffect();
        }
        if (stun == 0 && isPlaying)
        {
            DestroyStunEffect();
        }
    }

    void ApplyStunEffect()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject stunEffect = Instantiate(stunEffectPrefab, transform.position, Quaternion.identity);
            stunEffect.transform.parent = transform; // Follow parent
            stunEffect.GetComponent<Renderer>().sortingOrder = gameObject.GetComponent<Renderer>().sortingOrder;

            OrbitObject sheepOrbiter = stunEffect.GetComponent<OrbitObject>();
            sheepOrbiter.target = transform;

            // Offset pos of orbiter object
            sheepOrbiter.positionOffset = new Vector3(0, 0.5f, 0);

            // Init angle of each orbiter object
            float angleOffset = (360f / 3f) * i; // Divide orbit in 3
            sheepOrbiter.SetInitialAngle(angleOffset);

            // Optionnel: Détruire l'effet après la durée de stun
            stunAnimationList.Add(stunEffect);
        }
        isPlaying = true;
    }

    void DestroyStunEffect()
    {
        foreach (GameObject stunEffect in stunAnimationList)
        {
            Destroy(stunEffect);
        }
        stunAnimationList.Clear();
        isPlaying = false;
    }
}