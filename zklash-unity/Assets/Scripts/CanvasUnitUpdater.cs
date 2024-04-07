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

    public int unitLevel = 1;
    public string itemName; // TODO get name from entity
    public int itemLevel = 1;
    private Image itemImage;
    private TextMeshProUGUI itemLevelText;
    private TextMeshProUGUI itemNameText;
    private TextMeshProUGUI itemDescription;

    private string prefabName;

    private Image ribbon;
    private Image secondRibbon;

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
        ribbon = transform.Find("Ribbon").GetComponent<Image>();
        secondRibbon = transform.Find("SecondRibbon").GetComponent<Image>();

        Debug.Log("Unit prefab name: " + ribbon);
        Debug.Log("Unit prefab name: " + secondRibbon);
        SetupUIBasedOnPrefabName();
        SetupItemUIBasedOnPrefabName();
    }

    void Update()
    {
        ToggleRibbonsBasedOnItem();
    }

    public void ToggleRibbonsBasedOnItem()
    {
        if (string.IsNullOrEmpty(itemName))
        {
            // Si itemName est nul ou vide, activez SecondRibbon et désactivez Ribbon
            secondRibbon.gameObject.SetActive(true);
            ribbon.gameObject.SetActive(false);
            transform.Find("ItemInfo").gameObject.SetActive(false);
        }
        else
        {
            // Si itemName a une valeur, activez Ribbon et désactivez SecondRibbon
            ribbon.gameObject.SetActive(true);
            secondRibbon.gameObject.SetActive(false);
        }
    }

    private Sprite GetSpriteByName(string spriteName)
    {
        return Resources.Load<Sprite>("Sprites/" + spriteName);
    }

    private void SetupItemUIBasedOnPrefabName()
    {
        string description = GetItemDescriptionBasedOnLevelAndPrefab(itemName, itemLevel);

        Sprite itemSprite = GetSpriteByName(itemName + "_" + itemLevel); // Exemple de nommage de sprite

        SetItemInfo(itemSprite, itemLevel.ToString(), itemName, description);
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

    private string GetItemDescriptionBasedOnLevelAndPrefab(string itemName, int level)
    {
        itemName = itemName.ToLower();
        Debug.Log("itemName : " + itemName);
        Debug.Log("itemName : " + itemName.Contains("mushroom"));
        if (itemName.Contains("mushroom"))
        {
            switch (level)
            {
                case 1: return "Give +1 health when equipped.";
                case 2: return "Give +2 health when equipped.";
                case 3: return "Give +3 health when equipped.";
                default: return "No description for this level.";
            }
        }
        else if (itemName.Contains("rock"))
        {
            switch (level)
            {
                case 1: return "Deal +1 damage once when engaging the fight.";
                case 2: return "Deal +2 damage once when engaging the fight.";
                case 3: return "Deal +3 damage once when engaging the fight.";
                default: return "No description for this level.";
            }
        }
        else if (itemName.Contains("bush"))
        {
            switch (level)
            {
                case 1: return "Absorb 1 damage once.";
                case 2: return "Absorb 2 damage once.";
                case 3: return "Absorb 3 damage once.";
                default: return "No description for this level.";
            }
        }
        else if (itemName.Contains("pumpkin"))
        {
            switch (level)
            {
                case 1: return "At death, revive with 1 health point, once.";
                case 2: return "At death, revive with 1 health point, twice.";
                default: return "No description for this level.";
            }
        }

        return "No description available.";
    }
    public void SetUnitInfo(Sprite image, string level, string name, string description)
    {
        if (unitImageLeft != null) unitImageLeft.sprite = image;
        if (unitImageRight != null) unitImageRight.sprite = image;
        if (unitLevelText != null) unitLevelText.text = level;
        if (unitNameText != null) unitNameText.text = name;
        if (unitDescription != null) unitDescription.text = description;
    }

    public void SetItemInfo(Sprite image, string level, string name, string description)
    {
        if (itemImage != null) itemImage.sprite = image;
        if (itemLevelText != null) itemLevelText.text = GetSizeTextFromLevel(int.Parse(level));
        if (itemNameText != null) itemNameText.text = name;
        if (itemDescription != null) itemDescription.text = description;
    }

    private string GetSizeTextFromLevel(int level)
    {
        switch (level)
        {
            case 1: return "XS";
            case 2: return "M";
            case 3: return "XL";
            default: return "Unknown"; // ou retournez une valeur par défaut si cela a du sens dans votre contexte
        }
    }
}
