using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using zKlash.Game;
using zKlash.Game.Items;

public class MobItem : MonoBehaviour
{
    private MobController mobController;

    private CanvasUnitUpdater canvasUnitUpdater;

    public ItemData item;
    public TextMeshProUGUI titleItem;
    public TextMeshProUGUI sizeItem;
    public TextMeshProUGUI descriptionItem;
    private ItemData previousItem;
    public Image itemImage;

    public GameObject orbitObjectPrefab;

    public GameObject itemOrbiterGO;

    public string title;
    public char size;
    public string description;
    public int health;
    public int attack;
    public int damage;
    public int absorb;
    public int save;
    public int durability;

    private void Start()
    {
        // Get the MobController component from the GameObject
        mobController = GetComponent<MobController>();

        if (mobController == null)
        {
            Debug.LogError("MobController component not found on the GameObject.", this);
            return;
        }

        // Get the CanvasUnitUpdater component from the GameObject
        Transform canvasUnitsTransform = transform.Find("CanvasUnits");
        if (canvasUnitsTransform != null)
        {
            canvasUnitUpdater = canvasUnitsTransform.GetComponent<CanvasUnitUpdater>();
        }

        titleItem.text = "";
        sizeItem.text = "";
        descriptionItem.text = "";
    }

    private void Update()
    {
        if (item != previousItem)
        {
            Debug.Log("Item changed");
            Destroy(itemOrbiterGO);
            previousItem = item;
            UpdateGameObjectWithItemData();
            UpdateItemBannerUI(description, title, size);
            itemOrbiterGO = Instantiate(orbitObjectPrefab, gameObject.transform.position, Quaternion.identity);
            OrbitObject itemOrbiter = itemOrbiterGO.GetComponent<OrbitObject>();
            itemOrbiter.target = gameObject.transform;
            itemOrbiterGO.GetComponent<SpriteRenderer>().sprite = itemImage.sprite;

            if (canvasUnitUpdater != null)
            {
                canvasUnitUpdater.ToggleRibbons(true);
            }

        }
    }

    private void OnDisable()
    {
        if (itemOrbiterGO != null)
            itemOrbiterGO.SetActive(false);
    }

    private void OnEnable()
    {
        if (itemOrbiterGO != null)
            itemOrbiterGO.SetActive(true);
    }

    private void UpdateGameObjectWithItemData()
    {
        if (item != null)
        {
            title = item.title;
            size = item.size;
            description = item.description;
            health = mobController.Character.Item.Health(Phase.OnEquip);
            attack = mobController.Character.Item.Attack(Phase.OnEquip);
            damage = mobController.Character.Item.Damage(Phase.OnEquip);
            absorb = mobController.Character.Item.Absorb(Phase.OnEquip);
            save = item.save;
            durability = item.durability;
            itemImage.sprite = item.image;
        }
    }

    private void UpdateItemBannerUI(string description, string title, char size)
    {
        if (descriptionItem != null && description != null)
            descriptionItem.text = description;
        if (titleItem != null && title != null)
            titleItem.text = title;
        if (sizeItem != null && size != null)
            sizeItem.text = size.ToString();
    }
}