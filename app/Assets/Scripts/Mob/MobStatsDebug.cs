using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MobStatsDebug : MonoBehaviour
{
    private MobController mobController;

    public TextMeshProUGUI stunMob;
    public TextMeshProUGUI absorbMob;


    public int stun;
    public int absorb;

    public void Start()
    {
        // Get the MobController component from the GameObject
        mobController = GetComponent<MobController>();

        if (mobController == null)
        {
            Debug.LogError("MobController component not found on the GameObject.", this);
            return;
        }

        UpdateGameObjectWithData();
    }

    public void Update()
    {
        UpdateGameObjectWithData();
        stunMob.text = "Stun: " + stun;
        absorbMob.text = "Absorb: " + absorb;
    }

    private void UpdateGameObjectWithData()
    {
        if (mobController != null && mobController.Character != null)
        {
            stun = mobController.Character.Stun;
            absorb = mobController.Character.Absorb;
        }
    }
}