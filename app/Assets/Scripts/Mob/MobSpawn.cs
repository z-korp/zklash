using UnityEngine;
using zKlash.Game.Roles;

/* 
    This script is responsible for spawning mobs in the Shop. 
    It is attached to the mob spawn points in the game.
    When the role of the mob is set, the mob is created at the spawn point.
*/
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
