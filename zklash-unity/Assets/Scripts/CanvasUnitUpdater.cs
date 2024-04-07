using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CanvasUnitUpdater : MonoBehaviour
{
    private Image unitImageLeft;
    private Image unitImageRight;
    private TextMeshProUGUI unitLevelText;
    private TextMeshProUGUI unitNameText;
    private TextMeshProUGUI unitDescription;

    public int unitLevel;
    private Image itemImage;
    private TextMeshProUGUI itemLevelText;
    private TextMeshProUGUI itemNameText;
    private TextMeshProUGUI itemDescription;

    private string prefabName;

    void Awake()
    {
        prefabName = transform.root.name;  // transform.root vous donnera le GameObject racine le plus haut de la hiérarchie.

        unitImageLeft = transform.Find("UnitInfo/UnitLeft").GetComponent<Image>();
        unitImageRight = transform.Find("UnitInfo/UnitRight").GetComponent<Image>();
        unitLevelText = transform.Find("UnitInfo/Lvl/LevelText").GetComponent<TextMeshProUGUI>();
        unitNameText = transform.Find("UnitInfo/Title").GetComponent<TextMeshProUGUI>();
        unitDescription = transform.Find("UnitInfo/Description").GetComponent<TextMeshProUGUI>();
        itemImage = transform.Find("ItemInfo/Unit").GetComponent<Image>();
        itemLevelText = transform.Find("ItemInfo/Lvl/LevelText").GetComponent<TextMeshProUGUI>();
        itemNameText = transform.Find("ItemInfo/Title").GetComponent<TextMeshProUGUI>();
        itemDescription = transform.Find("ItemInfo/Description").GetComponent<TextMeshProUGUI>();
        SetupUIBasedOnPrefabName();
    }

    private Sprite GetSpriteByName(string spriteName)
    {
        return Resources.Load<Sprite>("Sprites/" + spriteName);
    }

    private void SetupUIBasedOnPrefabName()
    {
        string description = GetDescriptionBasedOnLevelAndPrefab(prefabName, unitLevel);

        if (prefabName.Contains("Archer"))
        {
            Sprite unitSprite = GetSpriteByName("Archer_Blue");
            SetUnitInfo(unitSprite, unitLevel.ToString(), "ARCHER", description);
        }
        else if (prefabName.Contains("Warrior"))
        {
            Sprite unitSprite = GetSpriteByName("Warrior_Blue");
            SetUnitInfo(unitSprite, unitLevel.ToString(), "KNIGHT", description);
        }
        else if (prefabName.Contains("Pawn"))
        {
            Sprite unitSprite = GetSpriteByName("Pawn_Blue");
            SetUnitInfo(unitSprite, unitLevel.ToString(), "PAWN", description);
        }
    }

    private string GetDescriptionBasedOnLevelAndPrefab(string prefab, int level)
    {
        if (prefabName.Contains("Warrior"))
        {
            switch (level)
            {
                case 1: return "Get +1 health when an item is equipped.";
                case 2: return "Get +2 health when an item is equipped.";
                case 3: return "Get +3 health when an item is equipped.";
                default: return "No description for this level.";
            }
        }
        else if (prefabName.Contains("Archer"))
        {
            switch (level)
            {
                case 1: return "At death, stun the foe for 1 turn.";
                case 2: return "At death, stun the foe for 2 turns.";
                case 3: return "At death, stun the foe for 3 turns.";
                default: return "No description for this level.";
            }
        }
        else if (prefabName.Contains("Pawn"))
        {
            switch (level)
            {
                case 1: return "At death, add +1 attack and +1 health to the next friend.";
                case 2: return "At death, add +2 attack and +2 health to the next friend.";
                case 3: return "At death, add +3 attack and +3 health to the next friend.";
                default: return "No description for this level.";
            }
        }
        else
        {
            return "No description available.";
        }
    }

    public void SetUnitInfo(Sprite image, string level, string name, string description)
    {
        if (unitImageLeft != null) unitImageLeft.sprite = image;
        if (unitImageRight != null) unitImageRight.sprite = image;
        if (unitLevelText != null) unitLevelText.text = level;
        if (unitNameText != null) unitNameText.text = name;
        if (unitDescription != null) unitDescription.text = description;
    }

    public void SetItemInfo(Sprite image, string level, string name)
    {
        if (itemImage != null) itemImage.sprite = image;
        if (itemLevelText != null) itemLevelText.text = level;
        if (itemNameText != null) itemNameText.text = name;
    }

    // Méthode pour appeler pour mettre à jour les UI
    // public void UpdateUI(Sprite unitSprite, string unitLevel, string unitName,
    //                      Sprite itemSprite, string itemLevel, string itemName)
    // {
    //     SetUnitInfo(unitSprite, unitLevel, unitName);
    //     SetItemInfo(itemSprite, itemLevel, itemName);
    // }
}
