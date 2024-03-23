using System;
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
        UpdateCounter(Data.StackSize);
        Data.UpdateStackSize += UpdateCounter;

        if (Data.Texture != null)
            if(Data.Texture.Atlas != null)
                sprite.Texture = Data.Texture;
            else
                LoadErrorTexture();
        else
            LoadErrorTexture();
    }



    public override void _ExitTree() => Data.UpdateStackSize -= UpdateCounter;



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
            GD.Print("Test");
            followCursor = false;
            ZIndex = prevZIndex;
            inventory.MoveItem(this);
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



    public void CenterInSlot() => GlobalPosition = GetParent<Slot>().Center - Size / 2;



    private void UpdateCounter(int stackSize)
    {
        if (stackSize < 1)
            QueueFree();

        counter.Text = stackSize.ToString();
    }

}
