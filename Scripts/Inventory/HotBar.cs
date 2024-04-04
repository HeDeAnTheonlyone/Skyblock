using Godot;
using System.Linq;



public partial class HotBar : Control
{
	private HBoxContainer itemList;



	public override void _Ready()
	{
		itemList = GetNode<HBoxContainer>("Padding/ItemList");
	}



    public Slot[] GetSlots() => itemList.GetChildren().Cast<Slot>().ToArray();
}
