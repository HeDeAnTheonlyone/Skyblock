using System.Linq;
using Godot;



public partial class PlayerInventory : Inventory
{
	[Export] public new InventoryPlayerData Data { get; private set; } = new InventoryPlayerData();
	private VBoxContainer armorList;
	private Slot[] armorSlots;
	public static InventoryItem movedItem;



	public override void _Ready()
	{
		base._Ready();
	}



	protected override bool IsDataNull() => Data == null;



	protected override void GetSlotParentNodes()
	{
		itemGrid = GetNode<GridContainer>("Padding/InnerPadding/Splitter/ItemGrid");
		armorList = GetNode<VBoxContainer>("Padding/InnerPadding/Splitter/PlayerSprite/ArmorPadding/ArmorList");
	}



    protected override void GetSlots()
    {
        base.GetSlots();
		armorSlots = armorList.GetChildren().Cast<Slot>().ToArray();
	}



    public override void _Input(InputEvent @event)
	{
		if (@event.IsActionReleased("Inventory"))
		{
			if (Visible)
				Close();
			else
				Open();
        }
	}



	public override void Open()
	{
		base.Open();

		Data.DisplayItemsInSlots(itemSlots, Data.Items, armorSlots, Data.Armor);
	}
}
