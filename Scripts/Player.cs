using Godot;

public partial class Player : RigidBody2D
{
	[ExportCategory("Movement")]
	[Export] public float MoveSpeed { get; set; }
	private Vector2 moveDir;
	private bool alive = true;



	public override void _Ready()
	{
	
	}

	
	
	public override void _Process(double delta)
	{
		
	}



	public override void _PhysicsProcess(double delta)
	{
		if (alive)
			Movement();
	}




    private void Movement()
	{
		moveDir.X = Input.GetActionStrength("MoveRight") - Input.GetActionStrength("MoveLeft");
		moveDir.Y = Input.GetActionStrength("MoveDown") - Input.GetActionStrength("MoveUp");

		Vector2 motion = Vector2.Zero.DirectionTo(moveDir) * MoveSpeed;

		MoveAndCollide(motion);
	}



	private void Death(Node body)
	{
		Tween tween = CreateTween();
		tween.TweenProperty(this, "scale", Vector2.Zero, 1);
		alive = false;
	}
}
