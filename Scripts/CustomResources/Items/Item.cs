using Godot;



[GlobalClass]
public partial class Item : Resource
{
    public AtlasTexture Texture { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    private int StackSize { get; set; }


    public Item(AtlasTexture texture, string name, string description)
    {
        Texture = texture;
        Name = name;
        Description = description;
    }
}