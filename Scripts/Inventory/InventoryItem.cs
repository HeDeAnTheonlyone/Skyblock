using Godot;



public partial class InventoryItem : Control
{
    [Export] public ItemData Data { get; set; }
    private InventoryData inventory;
    private Tooltip tooltip;
    private AnimatedTextureRect sprite;
    private Label counter;
    private bool followCursor = false;
    private int prevZIndex;



    public override void _Ready()
    {
        sprite = GetNode<AnimatedTextureRect>("Padding/Sprite");
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
            Image atlasImage = Data.Texture.Atlas.GetImage();

            for (int i = 0; i < Data.Frames; i++)
            {
                anim.AddFrame("Idle", ImageTexture.CreateFromImage(atlasImage.GetRegion(rect)));
                rect.Position = rect.Position with { X = rect.Position.X + rect.Size.X };
            }

            sprite.SpriteFrames = anim;
            sprite.Animation = "Idle";
            if (Data.AutoPlayAnim) sprite.Play();
        }
        else
        {
            anim.AddFrame("Idle", Data.Texture);
            sprite.SpriteFrames = anim;
            sprite.Animation = "Idle";
        }
    }



    private void LoadErrorTexture()
    {
        GD.PrintErr($"Failed to load item texture for '{GetPath()}'!");
        ImageTexture errTex = GD.Load<ImageTexture>("res://Assets/Items/Debug.png");

        SpriteFrames anim = new SpriteFrames();

        anim.AddAnimation("Fallback");
        anim.AddFrame("Fallback", errTex);
        
        sprite.SpriteFrames = anim;
        sprite.Animation = "Fallback";
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



    public void SetupData(ItemData _item, InventoryData _inventory, Tooltip _tooltip)
    {
        Data = _item;
        inventory =_inventory;
        tooltip = _tooltip;
    }



    private void CenterInSlot() => GlobalPosition = GetParent<Slot>().Center - Size / 2;



    #region Signal 
    private void UpdateCounter(int stackSize)
    {
        if (stackSize < 1)
            QueueFree();

        counter.Text = stackSize.ToString();
    }



    private void SetTooltipItemReference() => tooltip.SetContents(this);



    private void DeactivateTooltip() => tooltip.Deactivate();
    #endregion
}
