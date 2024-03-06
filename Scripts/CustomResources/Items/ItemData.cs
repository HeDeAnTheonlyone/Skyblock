using Godot;



[GlobalClass, Icon("res://Assets/Icons/Item.svg")]
public partial class ItemData : Resource
{
    [ExportGroup("Item")]
    [ExportSubgroup("Texture")]
    [Export] public AtlasTexture Texture { get; set; }
    //
    [ExportSubgroup("Properties")]
    [Export] public string Name { get; set; }
    [Export] public string Description { get; set; }
    [Export] public int MaxStackSize { get; set; }
    [Export] public int StackSize { get; set; }



    public ItemData() {}



    public ItemData(AtlasTexture texture, string name, string description, int maxStackSize = 1)
    {
        Texture = texture;
        Name = name;
        Description = description;
        MaxStackSize = maxStackSize;
    }
}