using Godot;



public partial class InventoryItem : Control
{
    [Export] public ItemData Data { get; set; }
    public InventoryData inventory;
    private TextureRect sprite;
    private Label counter;
    private bool followCursor = false;
    private int prevZIndex;



    public override void _Ready()
    {
        sprite = GetNode<TextureRect>("Sprite");
        counter = GetNode<Label>("Counter"); 
        SetCounter(Data.StackSize);
        Data.UpdateStackSize += SetCounter;

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
        {
            prevZIndex = ZIndex;
            ZIndex++;
            followCursor = true;
        }

        if (@event.IsActionReleased("LeftClick"))
        {
            followCursor = false;
            ZIndex = prevZIndex;
            inventory.MoveItem(this);
            CenterInSlot();
        }
    }



    private void CenterInSlot() => GlobalPosition = GetParent<Slot>().Center - Size / 2;



    private void SetCounter(int stackSize) => counter.Text = stackSize.ToString(); 
}