using Godot;
using System.Linq;



public partial class HotBar : Control
{
	private HBoxContainer itemList;
	private Panel selection;
	private int selected = 0;
	private int Selected
	{ 
		get => selected;
		set
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
        if (@event.IsActionPressed("ScrollHotBarRight"))
			Selected ++;
        
		if (@event.IsActionPressed("ScrollHotBarLeft"))
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
