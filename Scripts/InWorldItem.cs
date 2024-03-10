using Godot;



public partial class InWorldItem : Node2D
{
    [Export] public ItemData Data { get; set; }
    private Sprite2D sprite;



    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite");
        sprite.Texture = Data.Texture;
    }
}