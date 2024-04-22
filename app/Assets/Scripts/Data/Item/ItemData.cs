using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    public int id;
    public string title;
    public char size;
    public string description;
    public int health;
    public int attack;
    public int damage;
    public int absorb;
    public int save;
    public int durability;
    public Sprite image;

}
