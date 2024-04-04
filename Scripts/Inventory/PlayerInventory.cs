using System.Linq;
using Godot;



public partial class PlayerInventory : Inventory
{
	//[Export] public new InventoryPlayerData Data { get; private set; } = new InventoryPlayerData();
	[Export] public LootTable TestTable; // TODO Remove test table
	private VBoxContainer armorList;
	private HotBar hotBar;



	private PlayerInventory()
	{
		Data = new InventoryPlayerData();
	}



	public override void _Ready()
	{
		hotBar = GetNode<HotBar>("../../Hud/HotBar");
		base._Ready();
	}



	protected override bool IsDataNull() => Data == null;
	protected override InventoryData GetData() => Data;



    protected override void GetSlotParentNodes()
	{
		itemGrid = GetNode<GridContainer>("Padding/InnerPadding/Splitter/ItemGrid");
		armorList = GetNode<VBoxContainer>("Padding/InnerPadding/Splitter/PlayerSprite/ArmorPadding/ArmorList");
	}



    protected override void SetSlots() => (Data as InventoryPlayerData).SetSlots
	(
		itemGrid.GetChildren().Cast<Slot>().ToArray(),
		armorList.GetChildren().Cast<Slot>().ToArray(),
		hotBar.GetSlots()
	);



    public override void _Input(InputEvent @event)
	{
		if (@event.IsActionReleased("Inventory"))
		{
			if (Visible)
				Close();
			else
				Open();
        }

		// TODO Remove temporary inputs
		if (Input.IsActionJustPressed("Space"))
        {
			GD.Print("Add test items");
			Data.AddItem(TestTable.PlainLoot());
        }

		if (Input.IsPhysicalKeyPressed(Key.Backspace))
        {
			foreach (ItemData item in Data.Items)
			{
				if (item == null)
					continue;

				GD.Print(item.Name + "   " + item.StackSize + "/" + item.MaxStackSize);
			}
        }
		//
    }
}
