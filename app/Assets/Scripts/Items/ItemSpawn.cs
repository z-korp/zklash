using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    public Item item;
    private GameObject itemObject;
    
    void Update()
    {
        if (item != Item.None)
        {
            if (itemObject != null)
            {
                Destroy(itemObject);
            }
            itemObject = ItemManager.instance.Create(item, transform);
            int index = ShopManager.instance.IndexOfItemSpawn(this);
            ItemDraggable itemDraggable = itemObject.GetComponent<ItemDraggable>();
            if (index != -1)
            {
                itemDraggable.isFromShop = true;
                itemDraggable.indexFromShop = (uint)index;
            } else {
                itemDraggable.isFromShop = false;
            }
            item = Item.None;
        }
    }

    public void SetItem(Item _item)
    {
        item = _item;
    }
}
