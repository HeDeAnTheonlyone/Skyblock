using Godot;



[GlobalClass]
public partial class Placable : Item
{
    public int PlaceLayer { get; private set; }

    public Placable(ImageTexture texture, string name, string description) : base(texture, name, description)
    {

    }
}