using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public ItemData itemData;

    public string Title { get; private set; }
    public char Size { get; private set; }
    public string Description { get; private set; }
    public int Save { get; private set; }
    public int Durability { get; private set; }
    public int Price { get; private set; }
    public Sprite Image { get; private set; }

    protected virtual void Awake()
    {
        if (itemData != null)
        {
            InitializeFromItemData();
        }
        else
        {
            Debug.LogError("ItemData is null");
        }
    }

    protected void InitializeFromItemData()
    {
        Title = itemData.title;
        Size = itemData.size;
        Description = itemData.description;
        Save = itemData.save;
        Durability = itemData.durability;
        Price = itemData.price;
        Image = itemData.image;
    }

    public abstract void DisplayInfo();

    public override string ToString()
    {
        return $"Title: {Title}, Size: {Size}, Description: {Description}, Save: {Save}, Durability: {Durability}";
    }
}