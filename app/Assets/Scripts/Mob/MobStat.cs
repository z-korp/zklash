using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MobStat : MonoBehaviour
{
    public MobData mob;
    public TextMeshProUGUI titleMob;
    public TextMeshProUGUI levelMob;
    public TextMeshProUGUI descriptionMob;
    public Image imageLeftMob, imageRightMob;

    public string title;
    public int currentLevel = 1;
    public string powerLV1;
    public string powerLV2;
    public string powerLV3;
    public int health;
    public int damage;

    public void Start()
    {
        UpdateGameObjectWithItemData();
        descriptionMob.text = powerLV1;
        levelMob.text = currentLevel.ToString();
    }

    private void UpdateGameObjectWithItemData()
    {
        if (mob != null)
        {
            title = mob.title;
            powerLV1 = mob.powerLV1;
            powerLV2 = mob.powerLV2;
            powerLV3 = mob.powerLV3;
            health = mob.health;
            damage = mob.damage;
            imageLeftMob.sprite = mob.image;
            imageRightMob.sprite = mob.image;
        }
    }

    public void UpdateMobLevelBannerUI(int currentLevel)
    {
        levelMob.text = currentLevel.ToString();
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