using UnityEngine;
using zKlash.Game.Items;

public class ItemSpawn : MonoBehaviour
{
    public Item item;
    private GameObject itemObject;

    void Update()
    {
        if (item != Item.None)
        {
            itemObject = ItemManager.instance.Create(item, transform);
            int index = ShopManager.instance.IndexOfItemSpawn(this);
            ItemDraggable itemDraggable = itemObject.GetComponent<ItemDraggable>();
            if (index != -1)
            {
                itemDraggable.isFromShop = true;
                itemDraggable.index = index;
            }
            else
            {
                itemDraggable.isFromShop = false;
            }
            item = Item.None;
        }
    }

    public void SetItem(Item _item)
    {
        // Destroy the mob if it has not been bought
        if (itemObject != null)
        {
            if (itemObject.GetComponent<ItemDraggable>().isFromShop)
            {
                Destroy(itemObject);
            }
        }
        item = _item;
    }
}
