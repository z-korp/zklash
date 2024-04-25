using UnityEngine;
using zKlash.Game.Roles;

public class MobSpawn : MonoBehaviour
{
    public Role role;
    private GameObject mobObject;

    void Update()
    {
        if (role != Role.None)
        {
            mobObject = MobManager.instance.Create(role, transform);
            int index = ShopManager.instance.IndexOfMobSpawn(this);
            MobDraggable mobDraggable = mobObject.GetComponent<MobDraggable>();
            if (index != -1)
            {
                mobDraggable.isFromShop = true;
                mobDraggable.index = index;
            }
            else
            {
                mobDraggable.isFromShop = false;
            }
            role = Role.None;
        }
    }

    public void SetRole(Role _role)
    {
        // Destroy the mob if it has not been bought
        if (mobObject != null)
        {
            if (mobObject.GetComponent<MobDraggable>().isFromShop)
            {
                Destroy(mobObject);
            }
        }
        role = _role;
    }
}
