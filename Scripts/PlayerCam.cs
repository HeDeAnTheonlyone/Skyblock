using Godot;

public partial class PlayerCam : Camera2D
{
	private Player ownerPlayer;



	public override void _Ready()
	{
		ownerPlayer = GetParent<Player>();
	}



	public override void _PhysicsProcess(double delta)
	{
		Position = Vector2.Zero;
		// TODO add method for moving slightl towards the cursor
	}



    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ZoomIn"))
			Zoom += new Vector2(0.3f, 0.3f);
		
        if (@event.IsActionPressed("ZoomOut"))
			if (Zoom.X - 0.3f > 0.01f)
				Zoom -= new Vector2(0.3f, 0.3f);
		
    }
}
