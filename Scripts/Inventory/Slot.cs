using Godot;



public partial class Slot : AspectRatioContainer
{
    [Export] public SlotTypes SlotType { get; set; }
    public Vector2 Center
    {
        get => GlobalPosition + Size / 2;
    }



    public enum SlotTypes
    {
        Item,
        Helmet,
        ChestPlate,
        Leggins,
        Fuel
    }



    public bool MatchType(SlotTypes type)
    {
        if (SlotType == SlotTypes.Item)
            return true;

        return SlotType == type;
    }
}
