using UnityEngine;

public class MobItem : MonoBehaviour
{
    public ItemData item;
    private ItemData previousItem;


    public int health;
    public int attack;
    public int damage;
    public int absorb;
    public int save;
    public int durability;
    private void Update()
    {

        if (item != previousItem)
        {
            Debug.Log("Item changed");
            previousItem = item;
            UpdateGameObjectWithItemData();
        }
    }
    private void UpdateGameObjectWithItemData()
    {
        if (item != null)
        {
            health = item.health;
            attack = item.attack;
            damage = item.damage;
            absorb = item.absorb;
            save = item.save;
            durability = item.durability;
        }
    }

}