using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using zKlash.Game;
using zKlash.Game.Items;

public class MobItem : MonoBehaviour
{
    public IItem Item { get; set; }

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
        }
    }

    private void UpdateGameObjectWithItemData()
    {
        if (item != null)
        {
            title = item.title;
            size = item.size;
            description = item.description;
            health = item.health;
            attack = item.attack;
            damage = item.damage;
            absorb = item.absorb;
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