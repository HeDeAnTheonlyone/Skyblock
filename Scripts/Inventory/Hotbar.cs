using Godot;
using System.Linq;



public partial class Hotbar : Control
{
	private HBoxContainer itemList;
	private Panel selection;
	private int selected = 0;
	public int Selected
	{ 
		get => selected;
		private set
		{
			selected = value;
			
			if (Selected > 9)
				selected = 0;
			else if (Selected < 0)
				selected = 9;

			MoveSelection();
		}
	}



	public override void _Ready()
	{
		itemList = GetNode<HBoxContainer>("Padding/ItemList");
		selection = GetNode<Panel>("Selection");

		CallDeferred("MoveSelection");
	}



    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ScrollHotbarRight") && !Input.IsActionPressed("Alt"))
			Selected ++;
        
		if (@event.IsActionPressed("ScrollHotbarLeft") && !Input.IsActionPressed("Alt"))
			Selected --;
    }



    private void MoveSelection()
	{
		Slot slot = itemList.GetNode<Slot>($"Slot{Selected}");
		selection.GlobalPosition = slot.GetVisualPosition();
		selection.Size = slot.getVisualSize();
	}



    public Slot[] GetSlots() => itemList.GetChildren().Cast<Slot>().ToArray();
}
