using Godot;


public partial class Inventory : CanvasLayer
{



	public override void _Ready()
	{
		Visible = false;
	}



	public override void _Process(double delta)
	{
	}



    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Inventory"))
		{
			if (Visible)
			{
				GetTree().Paused = false;
				Visible = false;
				Input.MouseMode = Input.MouseModeEnum.Hidden;
			}
			else
			{
				GetTree().Paused = true;
				Visible = true;
				Input.MouseMode = Input.MouseModeEnum.Visible;
			}
		}
    }
}
