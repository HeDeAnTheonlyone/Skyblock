using Godot;



public partial class Item : Control
{
    [Export] public ItemData ItemType { get; private set; }
}