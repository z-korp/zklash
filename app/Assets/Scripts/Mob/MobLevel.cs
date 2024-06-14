
using TMPro;
using UnityEngine;

public class MobLevel : MonoBehaviour
{
    private MobController mobController;

    private MobStat mobStat;

    public XPBar xpBar;

    public TextMeshProUGUI levelText;

    public int Level
    {
        get
        {
            if (mobController == null)
            {
                Debug.LogError("MobController is not set on " + gameObject.name, this);
                return 0;
            }
            return mobController.Character != null ? mobController.Character.Level : 0;
        }
    }

    public int XP
    {
        get
        {
            if (mobController == null)
            {
                Debug.LogError("MobController is not set on " + gameObject.name, this);
                return 0;
            }
            return mobController.Character != null ? mobController.Character.XP : 0;
        }
    }

    void Start()
    {
        // Get the MobController component from the GameObject
        mobController = GetComponent<MobController>();
        if (mobController == null)
        {
            Debug.LogError("MobController component not found on the GameObject.", this);
            return;
        }

        // Get the MobStat component from the GameObject
        mobStat = GetComponent<MobStat>();
        if (mobStat == null)
        {
            Debug.LogError("MobStat component not found on the GameObject.", this);
            return;
        }

        xpBar.SetMaxXP(100);
    }



    // Update is called once per frame
    void Update()
    {

        // display Xp when clicking space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Level: " + Level + "XP: " + XP + " / " + mobController.Character.ExperienceRequiredForLevel(Level));
        }

        xpBar.SetXP(XP * 100 / mobController.Character.ExperienceRequiredForLevel(Level));
        mobStat.UpdateMobLevelBannerUI(Level);
        mobStat.UpdateMobPowerBannerUI(Level);
        levelText.text = "LVL " + Level;
    }
}
