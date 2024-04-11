using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    public int id;
    public string title;
    public int health;
    public int attack;
    public int damage;
    public int absorb;
    public Sprite image;
    public Item item;

}
