using Godot;



[GlobalClass, Icon("res://Assets/Icons/Item.svg")]
public abstract partial class ItemData : Resource
{
    [ExportGroup("Item")]
    [ExportSubgroup("Texture"),]
    [Export] public AtlasTexture Texture { get; protected set; }
    [Export] public int Frames { get; protected set; } = 1;
    [Export] public bool AutoPlayAnim { get; protected set; } = false;
    //
    [ExportSubgroup("Properties")]
    [Export] public string Name { get; protected set; }
    [Export] public StringName Id { get; protected set; }
    [Export] public string Description { get; protected set; }
    [Export] public int MaxStackSize { get; protected set; } = 1;
    private int stackSize = 1;
    [Export] public int StackSize
    {
        get => stackSize;
        set
        {
            stackSize = Mathf.Clamp(value, 0, MaxStackSize);
            EmitSignal(SignalName.UpdateStackSize, StackSize);
        }
    }

    [Export] public Slot.SlotTypes SlotType { get; protected set; }
    [Export] public ItemRarity Rarity { get; protected set; }



    public ItemData() {}



    public ItemData(AtlasTexture texture, string name, StringName id, string description, int maxStackSize)
    {
        Texture = texture;
        Name = name;
        Id = id;
        Description = description;
        MaxStackSize = maxStackSize;
    }



    public bool CanMergeWith(ItemData item) => item.Id == Id && StackSize < MaxStackSize;



    [Signal] public delegate void UpdateStackSizeEventHandler(int value);



    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Legendary
    }
}