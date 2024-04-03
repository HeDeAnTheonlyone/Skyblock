using Godot;



public partial class InventoryItem : Control
{
    [Export] public ItemData Data { get; set; }
    public InventoryData inventory;
    // private TextureRect sprite;
    private AnimatedTextureRect sprite;
    private Label counter;
    private bool followCursor = false;
    private int prevZIndex;



    public override void _Ready()
    {
        sprite = GetNode<AnimatedTextureRect>("Sprite");
        counter = GetNode<Label>("Counter"); 
        UpdateCounter(Data.StackSize);
        Data.UpdateStackSize += UpdateCounter;

        if (Data.Texture != null)
            if(Data.Texture.Atlas != null)
                GenerateAnimationFrames();
            else
                LoadErrorTexture();
        else
            LoadErrorTexture();
    }



    public override void _ExitTree() => Data.UpdateStackSize -= UpdateCounter;



    private void GenerateAnimationFrames()
    {
        SpriteFrames anim = new SpriteFrames();
        anim.AddAnimation("Idle");

        if (Data.Frames > 1)
        {
            Rect2I rect = (Rect2I)Data.Texture.Region;
            Image altasImage = Data.Texture.Atlas.GetImage();

            for (int i = 0; i < Data.Frames; i++)
            {
                anim.AddFrame("Idle", ImageTexture.CreateFromImage(altasImage.GetRegion(rect)));
                rect.Position = rect.Position with { X = rect.Position.X + rect.Size.X + GameManager.Instance.TileOffSet };
            }

            sprite.SpriteFrames = anim;
            sprite.Play("Idle");
        }
        else
        {
            anim.AddFrame("Idle", Data.Texture);
            sprite.SpriteFrames = anim;
            sprite.Animation = "Idle";
        }
    }



    // FIX
    private void LoadErrorTexture()
    {
        GD.PrintErr($"Failed to load item texture for '{GetPath()}'!");
        AtlasTexture errTex = new AtlasTexture
        {
            Atlas = GD.Load<Texture2D>("res://Assets/Items/Debug.png"),
            Region = new Rect2(0, 0 , 16, 16)
        };
        // sprite.Texture = errTex;
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

        if (@event.IsActionPressed("RightClick") && followCursor)
        {
            inventory.SplitItem(this, 1);
        }

        if (@event.IsActionPressed("ShiftRightClick") && followCursor)
        {
            inventory.SplitItem(this, Data.StackSize / 2);
        }
    }



    private void CenterInSlot() => GlobalPosition = GetParent<Slot>().Center - Size / 2;



    #region Signal 
    private void UpdateCounter(int stackSize)
    {
        if (stackSize < 1)
            QueueFree();

        counter.Text = stackSize.ToString();
    }
    #endregion
}
