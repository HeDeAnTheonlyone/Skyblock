using System.Linq;
using Godot;



public partial class PlayerInventory : Inventory
{
	[Export] public new InventoryPlayerData Data { get; private set; } = new InventoryPlayerData();
	private VBoxContainer armorList;



	public override void _Ready()
	{
		base._Ready();
	}



	protected override bool IsDataNull() => Data == null;
    protected override InventoryData GetData() => Data;



    protected override void GetSlotParentNodes()
	{
		itemGrid = GetNode<GridContainer>("Padding/InnerPadding/Splitter/ItemGrid");
		armorList = GetNode<VBoxContainer>("Padding/InnerPadding/Splitter/PlayerSprite/ArmorPadding/ArmorList");
	}



    protected override void GetSlots()
    {
		Data.ItemSlots = itemGrid.GetChildren().Cast<Slot>().ToArray();
		Data.ArmorSlots = armorList.GetChildren().Cast<Slot>().ToArray();
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
		DisplayItemsInSlots(Data.ItemSlots, Data.Items);
		DisplayItemsInSlots(Data.ArmorSlots, Data.Armor);
	}
}
