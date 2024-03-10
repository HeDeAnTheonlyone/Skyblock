using System.Linq;
using Godot;



public partial class PlayerInventory : Inventory
{
	[Export] public InventoryPlayerData Data { get; private set; } = new InventoryPlayerData();
	private GridContainer itemGrid;
	private VBoxContainer armorList;



	public override void _Ready()
	{
		Visible = false;

		if (Data == null)
			GD.PrintErr($"Node '{GetPath()}' has no Inventory resource!");
		
		itemGrid = GetNode<GridContainer>("Padding/InnerPadding/Splitter/ItemGrid");
		armorList = GetNode<VBoxContainer>("Padding/InnerPadding/Splitter/PlayerSprite/ArmorPadding/ArmorList");
	}



    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Inventory"))
		{
			if (Visible)
				Close();
			else
				Open();
		}
    }



	private void Open()
	{
		GetTree().Paused = true;
		Visible = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;

		Slot[] itemSlots = itemGrid.GetChildren().Cast<Slot>().ToArray();
		Slot[] armorSlots = armorList.GetChildren().Cast<Slot>().ToArray();
		Data.AddItemsToSlots(itemSlots, Data.Items, armorSlots, Data.Armor);
	}



	private void Close()
	{
		GetTree().Paused = false;
		Visible = false;
		Input.MouseMode = Input.MouseModeEnum.Hidden;
	}
}
