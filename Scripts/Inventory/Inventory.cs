using System.Linq;
using Godot;



public abstract partial class Inventory : CanvasLayer
{
	public virtual InventoryData Data { get; set; }
	protected GridContainer itemGrid;
	protected Slot[] itemSlots;



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



	protected abstract void GetSlotParentNodes();



    protected virtual void GetSlots() => itemSlots = itemGrid.GetChildren().Cast<Slot>().ToArray();


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
}
