using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawn : MonoBehaviour
{
    public Role role;
    private GameObject mobObject;
    
    void Update()
    {
        if (role != Role.None)
        {
            if (mobObject != null)
            {
                Destroy(mobObject);
            }
            mobObject = MobManager.instance.Create(role, transform);
            int index = ShopManager.instance.IndexOfMobSpawn(this);
            MobDraggable mobDraggable = mobObject.GetComponent<MobDraggable>();
            if (index != -1)
            {
                mobDraggable.isFromShop = true;
                mobDraggable.indexFromShop = (uint)index;
            } else {
                mobDraggable.isFromShop = false;
            }
            role = Role.None;
        }
    }

    public void SetRole(Role _role)
    {
        role = _role;
    }
}
