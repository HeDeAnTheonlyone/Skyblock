using Godot;



public abstract partial class Inventory : Control
{
	public InventoryData Data { get; protected set; }
	protected GridContainer itemGrid;
	protected Tooltip tooltip;



	public override void _Ready()
    {
		tooltip = GetNodeOrNull<Tooltip>("Tooltip");

		if (tooltip == null)
			throw new NodeIsMissingException($"Tooltip node at path '{GetPath()}/Tooltip' is missing!");

		GetSlotCollectionNodes();

		if (itemGrid == null)
			throw new FieldIsNullException($"No reference to an ItemGrid in '{GetPath()}'!");

		if (IsDataNull())
			throw new FieldIsNullException($"Node '{GetPath()}' has no Inventory resource!");

		SetSlotsAndValues();

		Visible = false;
	}



    protected virtual bool IsDataNull() => Data == null;



	protected virtual InventoryData GetData() => Data;



	protected abstract void GetSlotCollectionNodes();



    protected abstract void SetSlotsAndValues();



    public virtual void Open()
	{
		GetData().UpdateInventoryItems();
		GetTree().Paused = true;
		Visible = true;
		GameManager.ShowInWorldCursor(false);
	}



	public void Close()
	{
		GetTree().Paused = false;
		Visible = false;
		GameManager.ShowInWorldCursor(true);
	}
}
