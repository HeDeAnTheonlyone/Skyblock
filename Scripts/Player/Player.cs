using Godot;

public partial class Player : RigidBody2D
{
	public bool Alive { get; private set; } = true;
	[ExportCategory("Movement")]
	[Export] public float MoveSpeed { get; set; }
	public InventoryPlayerData Inventory { get; private set; }
	private Vector2 moveDir;
	private AnimatedSprite2D sprite;



	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("Sprite");	
		Inventory = GetNode<PlayerInventory>("Overlay/PlayerInventory").Data as InventoryPlayerData;
	}



    public override void _PhysicsProcess(double delta)
	{
		if (Alive)
		{
			Movement();	
			Animation();
		}
	}



    private void Movement()
	{
		moveDir.X = Input.GetActionStrength("MoveRight") - Input.GetActionStrength("MoveLeft");
		moveDir.Y = Input.GetActionStrength("MoveDown") - Input.GetActionStrength("MoveUp");

		Vector2 motion = Vector2.Zero.DirectionTo(moveDir) * MoveSpeed;

		MoveAndCollide(motion);
	}



	private void Animation()
	{
		if (moveDir == Vector2.Zero)
		{
			sprite.Stop();
			return;
		}

		if (Mathf.Abs(moveDir.X) > Mathf.Abs(moveDir.Y))
		{
			if (moveDir.X > 0)
				sprite.Play("WalkRight");
			else
				sprite.Play("WalkLeft");
		}
		else
		{
			if (moveDir.Y > 0)
				sprite.Play("WalkDown");
			else
				sprite.Play("WalkUp");
		}
	}



	public void CollectItem(ItemData item) => Inventory.AddItem(item);



	#region Signals
	private void Death(Node body)
	{
		Tween tween = CreateTween();
		tween.TweenProperty(this, "scale", Vector2.Zero, 1);
		Alive = false;
	}
	#endregion
}
