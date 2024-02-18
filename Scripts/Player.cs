using Godot;

public partial class Player : Node2D
{
	[ExportCategory("Movement")]
	private Vector2 moveDir;
	[Export] public float MoveSpeed { get; set; }



	public override void _Ready()
	{
		
	}

	
	
	public override void _Process(double delta)
	{
		
	}



	public override void _PhysicsProcess(double delta)
	{
		#region Movement
		moveDir = Vector2.Zero;

		if (Input.IsActionPressed("MoveUp"))
			moveDir.Y -= 1;

		if (Input.IsActionPressed("MoveDown"))
			moveDir.Y += 1;

		if (Input.IsActionPressed("MoveLeft"))
			moveDir.X -= 1;

		if (Input.IsActionPressed("MoveRight"))
			moveDir.X += 1;

		GD.Print(Vector2.Zero.DirectionTo(moveDir) * MoveSpeed);
		GD.Print();

		GlobalPosition += Vector2.Zero.DirectionTo(moveDir) * MoveSpeed;
		#endregion
	}
}
