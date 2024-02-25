using Godot;



[GlobalClass]
public partial class Item : Resource
{
    public ImageTexture Texture { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }


    public Item(ImageTexture texture, string name, string description)
    {
        Texture = texture;
        Name = name;
        Description = description;
    }
}