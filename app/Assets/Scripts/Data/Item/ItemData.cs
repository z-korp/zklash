using UnityEngine;
using zKlash.Game.Items;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    public Item type;
    public string title;
    public char size;
    public string description;
    public int save;
    public int durability;
    public Sprite image;

}
