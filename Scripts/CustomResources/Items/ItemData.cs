using Godot;



[GlobalClass, Icon("res://Assets/Icons/Item.svg")]
public abstract partial class ItemData : Resource
{
    [ExportGroup("Item")]
    [ExportSubgroup("Texture")]
    [Export] public virtual AtlasTexture Texture { get; set; }
    //
    [ExportSubgroup("Properties")]
    [Export] public string Name { get; set; }
    [Export] public string Description { get; set; }
    [Export] public virtual int MaxStackSize { get; set; }
    [Export] public int StackSize { get; set; }
    [Export] public ItemRarity Rarity { get; set; }



    public ItemData() {}



    public ItemData(AtlasTexture texture, string name, string description, int maxStackSize)
    {
        Texture = texture;
        Name = name;
        Description = description;
        MaxStackSize = maxStackSize;
    }



    public enum ItemRarity
    {
        Common,
        Rare,
        ExtremlyRare,
        Legendary
    }
}