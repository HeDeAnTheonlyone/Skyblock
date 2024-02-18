using Godot;

public partial class Player : RigidBody2D
{
	[ExportCategory("Movement")]
	[Export] public float MoveSpeed { get; set; }
	private Vector2 moveDir;



	public override void _Ready()
	{
		PackedScene camObj = GD.Load<PackedScene>("res://Objects/PlayerCam.tscn");
		PlayerCam cam = camObj.Instantiate<PlayerCam>();
		cam.Player = this;
		GetNode("..").CallDeferred("add_child", cam);
	}

	
	
	public override void _Process(double delta)
	{
		
	}



	public override void _PhysicsProcess(double delta)
	{
		#region Movement
		moveDir.X = Input.GetActionStrength("MoveRight") - Input.GetActionStrength("MoveLeft");
		moveDir.Y = Input.GetActionStrength("MoveDown") - Input.GetActionStrength("MoveUp");

		LinearVelocity = Vector2.Zero.DirectionTo(moveDir) * MoveSpeed;
		GD.Print(LinearVelocity);
		#endregion
	}
}
