using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MobStat : MonoBehaviour
{
    private MobController mobController;

    public MobData mob;
    public TextMeshProUGUI titleMob;
    public TextMeshProUGUI levelMob;
    public TextMeshProUGUI descriptionMob;
    public Image imageLeftMob, imageRightMob;

    public string title;
    public string powerLV1;
    public string powerLV2;
    public string powerLV3;
    public int health;
    public int attack;

    public void Start()
    {
        // Get the MobController component from the GameObject
        mobController = GetComponent<MobController>();

        if (mobController == null)
        {
            Debug.LogError("MobController component not found on the GameObject.", this);
            return;
        }

        UpdateGameObjectWithItemData();
        descriptionMob.text = powerLV1;
        levelMob.text = "lvl " + mobController.Character.Level.ToString();
        titleMob.text = title;
    }

    private void UpdateGameObjectWithItemData()
    {
        if (mob != null && mobController != null && mobController.Character != null)
        {
            title = mob.title;
            powerLV1 = mob.powerLV1;
            powerLV2 = mob.powerLV2;
            powerLV3 = mob.powerLV3;
            health = mobController.Character.Health;
            attack = mobController.Character.Attack;
            imageLeftMob.sprite = mob.image;
            imageRightMob.sprite = mob.image;
        }
    }

    public void UpdateMobLevelBannerUI(int currentLevel)
    {
        levelMob.text = "lvl " + currentLevel.ToString();
    }

    public void UpdateMobPowerBannerUI(int currentLevel)
    {
        if (currentLevel == 1)
            descriptionMob.text = powerLV1;
        if (currentLevel == 2)
            descriptionMob.text = powerLV2;
        if (currentLevel == 3)
            descriptionMob.text = powerLV3;
    }
}