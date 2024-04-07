using System.Linq;
using Godot;



public partial class PlayerInventory : Inventory
{
	[Export] public LootTable TestTable; // TODO Remove test table
	private VBoxContainer armorList;
	private Hotbar hotbar;



	private PlayerInventory()
	{
		Data = new InventoryPlayerData();
	}



	public override void _Ready()
	{
		base._Ready();
	}



	protected override bool IsDataNull() => Data == null;



	protected override InventoryData GetData() => Data;



    protected override void GetSlotCollectionNodes()
	{
		itemGrid = GetNode<GridContainer>("Padding/InnerPadding/Splitter/ItemGrid");
		armorList = GetNode<VBoxContainer>("Padding/InnerPadding/Splitter/PlayerSprite/ArmorPadding/ArmorList");
	}



    protected override void SetSlotsAndValues() => (Data as InventoryPlayerData).SetupData
	(
		tooltip,
		itemGrid.GetChildren().Cast<Slot>().ToArray(),
		armorList.GetChildren().Cast<Slot>().ToArray(),
		GetNode<Hotbar>("../../Hud/Hotbar")
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

		/// TEMP temporary inputs
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
		///
    }
}
