using Godot;



public partial class Slot : AspectRatioContainer
{
    [Export] public SlotTypes SlotType { get; set; }
    public Vector2 Center
    {
        get => GlobalPosition + Size / 2;
    }
    private Panel bg;



    public override void _Ready()
    {
        bg = GetNode<Panel>("Bg");
    }



    public enum SlotTypes
    {
        Item,
        All,
        Helmet,
        ChestPlate,
        Leggins,
        Fuel,
    }



    public bool MatchType(SlotTypes type)
    {
        if (type == SlotTypes.All || SlotType == SlotTypes.Item)
            return true;

        return SlotType == type;
    }



    public Vector2 GetVisualPosition() => bg.GlobalPosition;



    public Vector2 getVisualSize() => bg.Size;
}
