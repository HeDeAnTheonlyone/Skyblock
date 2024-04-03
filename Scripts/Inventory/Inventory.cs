using System.Linq;
using Godot;



public abstract partial class Inventory : Control
{
	public virtual InventoryData Data { get; set; }
	protected GridContainer itemGrid;



	public override void _Ready()
    {
		GetSlotParentNodes();

		if (itemGrid == null)
			throw new FieldIsNullException($"No reference to an itemgrid in '{GetPath()}'!");

		if (CheckInventoryData())
			throw new FieldIsNullException($"Node '{GetPath()}' has no Inventory resource!");

		//GetData().SetItemBufferNode(GetNode<Control>("ItemBuffer"));

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
		GetData().UpdateInventoryItems();
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
}
