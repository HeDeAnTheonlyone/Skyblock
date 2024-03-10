using Godot;



public partial class InventoryItem : Control
{
    [Export] public ItemData Data { get; set; }
    private TextureRect sprite;



    public override void _Ready()
    {
        sprite = GetNode<TextureRect>("Sprite");
        
        if (Data.Texture.Atlas != null)
            sprite.Texture = Data.Texture;
        else
        {
            AtlasTexture aTex = new AtlasTexture
            {
                Atlas = GD.Load<Texture2D>("res://Assets/DecorationsTiles.png"),
                Region = new Rect2(Vector2.Zero, 16, 16)
            };
            sprite.Texture = aTex;
        }
    }
}