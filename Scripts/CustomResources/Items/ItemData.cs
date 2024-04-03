using Godot;



[GlobalClass, Icon("res://Assets/Icons/Item.svg")]
public abstract partial class ItemData : Resource
{
    [ExportGroup("Item")]
    [ExportSubgroup("Texture")]
    [Export] public virtual AtlasTexture Texture { get; set; }
    [Export] public int Frames { get; set; } = 1;
    //
    [ExportSubgroup("Properties")]
    [Export] public string Name { get; set; }
    [Export] public string Description { get; set; }
    [Export] public int MaxStackSize { get; set; }
    private int stackSize;
    [Export] public int StackSize
    {
        get => stackSize;
        set
        {
            stackSize = Mathf.Clamp(value, 0, MaxStackSize);
            EmitSignal(SignalName.UpdateStackSize, stackSize);   
        }
    }

    [Export] public Slot.SlotTypes SlotType { get; set; }
    [Export] public ItemRarity Rarity { get; set; }



    public ItemData()
    {
        StackSize = 1;
    }



    public ItemData(AtlasTexture texture, string name, string description, int maxStackSize)
    {
        Texture = texture;
        Name = name;
        Description = description;
        MaxStackSize = maxStackSize;
    }



    public bool CanMergeWith(ItemData item) => item.Name == Name && StackSize < MaxStackSize;



    [Signal] public delegate void UpdateStackSizeEventHandler(int value);



    public enum ItemRarity
    {
        Common,
        Rare,
        ExtremlyRare,
        Legendary
    }
}