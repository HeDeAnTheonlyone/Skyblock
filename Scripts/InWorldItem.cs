using Godot;



public partial class InWorldItem : Node2D
{
    [Export] public ItemData Data { get; set; }
    private Sprite2D sprite;



    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite");
        sprite.Texture = Data.Texture;
        Data.UpdateStackSize += DeleteOnItemStackEmpty;
    }



    public override void _ExitTree() => Data.UpdateStackSize -= DeleteOnItemStackEmpty;



    private void CollectItem(Node2D body)
    {
        Player player = body as Player;
        
        if (player != null)
            player.CollectItem(Data);
    }



    #region Signals
    private void DeleteOnItemStackEmpty(int stackSize)
    {
        if (stackSize < 1)
            QueueFree();
    }
    #endregion
}