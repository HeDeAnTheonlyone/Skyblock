using Godot;



public partial class Tooltip : Control
{
	private Label name;
	private Label description;
	[Export] private bool active = false;



	public override void _Ready()
	{
		name = GetNode<Label>("TextFieldList/Name");
		description = GetNode<Label>("TextFieldList/Description");

		Visible = false;
	}



    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ShowTooltip") && active)
		{
			Visible = true;
		}

		if (@event.IsActionReleased("ShowTooltip"))
		{
			Visible = false;
		}
    }



	private void ResizeLables()
	{

		Font fnt = name.GetThemeDefaultFont();
		float newWidth = fnt.GetStringSize(name.Text, fontSize: name.GetThemeDefaultFontSize()).X + 100;

		name.CustomMinimumSize = name.CustomMinimumSize with { X = newWidth };
		description.CustomMinimumSize = description.CustomMinimumSize with { X = newWidth };
	}



	#region Signals
	public void Deactivate() => active = false;



    public void SetContents(InventoryItem item)
	{
		name.Text = item.Data.Name;
		description.Text = item.Data.Description;
		GlobalPosition = item.GlobalPosition;
		ResizeLables();
		active = true;
	}
	#endregion
}
