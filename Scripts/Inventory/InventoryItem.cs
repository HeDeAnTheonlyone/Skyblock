using Godot;



public partial class InventoryItem : Control
{
    [Export] public ItemData Data { get; set; }
    private TextureRect sprite;
    private bool followCursor = false;
    private readonly Vector2 defaultPos = new Vector2(-0.5f, 0.0f);



    public override void _Ready()
    {
        sprite = GetNode<TextureRect>("Sprite");
        
        if (Data.Texture != null)
            if(Data.Texture.Atlas != null)
                sprite.Texture = Data.Texture;
            else
                LoadErrorTexture();
        else
            LoadErrorTexture();
    }



    private void LoadErrorTexture()
    {
        GD.PrintErr($"Failed to load item texture for '{GetPath()}'!");
        AtlasTexture errTex = new AtlasTexture
        {
            Atlas = GD.Load<Texture2D>("res://Assets/Items/Debug.png"),
            Region = new Rect2(Vector2.Zero, 16, 16)
        };
        sprite.Texture = errTex;
    }



    public override void _Process(double delta)
    {
        if (followCursor)
            GlobalPosition = GetGlobalMousePosition() - Size / 2;
    }



    public override void _GuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("LeftClick"))
            followCursor = true;

        if (@event.IsActionReleased("LeftClick"))
        {
            followCursor = false;
            Position = defaultPos;
            PlayerInventory.movedItem = this;
        }
    }
}