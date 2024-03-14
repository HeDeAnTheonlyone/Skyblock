using System.Linq;
using Godot;



public abstract partial class Inventory : CanvasLayer
{
	public virtual InventoryData Data { get; set; }
	protected GridContainer itemGrid;
	private readonly PackedScene invItem = GD.Load("res://Objects/Inventory/InventoryItem.tscn") as PackedScene;



	public override void _Ready()
    {
		GetSlotParentNodes();

		if (itemGrid == null)
			throw new FieldIsNullException($"No reference to an itemgrid in '{GetPath()}'!");

		if (CheckInventoryData())
			GD.PrintErr($"Node '{GetPath()}' has no Inventory resource!");

		GetSlots();
		Visible = false;
	}



	private bool CheckInventoryData() => IsDataNull();
    protected virtual bool IsDataNull() => Data == null;
	protected virtual InventoryData GetData() => Data;



	protected abstract void GetSlotParentNodes();



    protected virtual void GetSlots() => Data.ItemSlots = itemGrid.GetChildren().Cast<Slot>().ToArray();



    public virtual void Open()
	{
		GetTree().Paused = true;
		Visible = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}



	public void Close()
	{
		GetTree().Paused = false;
		Visible = false;
		Input.MouseMode = Input.MouseModeEnum.Hidden;
	}



	public virtual void DisplayItemsInSlots(Slot[] slots, ItemData[] items)
	{
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i] == null)
			{
				if (slots[i].GetChildCount() > 1)
					slots[i].GetNode<InventoryItem>("InventoryItem").QueueFree();

				continue;
			}

			if (slots[i].GetChildCount() > 1)
			{
				InventoryItem item = slots[i].GetNode<InventoryItem>("InventoryItem");

				if (item.Data != items[i])
				{
					item.QueueFree();
					DisplayItemsInSlotsInstantiastor(slots[i], items[i]);
				}
			}
			else
				DisplayItemsInSlotsInstantiastor(slots[i], items[i]);
		}
	}



	protected virtual void DisplayItemsInSlotsInstantiastor(Slot slot, ItemData item)
	{
		InventoryItem invItemInstance = invItem.Instantiate() as InventoryItem;
		invItemInstance.Data = item;
		invItemInstance.inventory = GetData();
		slot.AddChild(invItemInstance);
	}
}
