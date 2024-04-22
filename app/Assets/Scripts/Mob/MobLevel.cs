
using TMPro;
using UnityEngine;

public class MobLevel : MonoBehaviour
{
    public MobData mobData;
    public XPBar xpBar;

    public TextMeshProUGUI levelText;

    public int currentLevel = 1;

    public int unitMerged = 1;

    public int xpAmount = 100;

    public int xpLevel = 0;

    private bool updateXP = true;

    private int LevelMax = 3;
    // Start is called before the first frame update
    void Start()
    {
        currentLevel = mobData.xp;
        xpBar.SetMaxXP(100);
        xpAmount = 100 / (currentLevel + 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (updateXP)
            xpBar.SetXP(xpLevel);
    }

    public bool LevelUp(int amountXP = 0)
    {
        // Remember to update number of unit merged before leveling up
        if (currentLevel < LevelMax)
        {
            // Avoid 99 XP case by using CeilToInt
            xpAmount = Mathf.CeilToInt(100f / ((float)currentLevel + 1));
            xpLevel += amountXP != 0 ? amountXP : xpAmount;

            if (xpLevel >= 100)
            {
                currentLevel += 1;
                levelText.text = "LVL " + currentLevel;
                GetComponent<MobStat>().UpdateMobLevelBannerUI(currentLevel);
                GetComponent<MobStat>().UpdateMobPowerBannerUI(currentLevel);
                xpLevel = 0;
                if (currentLevel == LevelMax)
                {
                    xpBar.SetXP(100);
                    xpBar.SetColor(Color.green);
                    updateXP = false;
                }
            }

            return true;
        }

        return false;

    }

}
