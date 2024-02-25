using System;
using Godot;

public partial class PreviewCursor : Node2D
{
	private Player ownerPlayer;
	private TileMap grid;
	private Sprite2D highlighter;
	private Sprite2D modeIcon;
	public PackedScene placableObject;
	private Vector2 mousePos;
	private int gridSize;
	private bool inCursorAnim;
	private CursorMode mode = CursorMode.Inspect;
	public CursorMode Mode
	{
		get => mode;
		set
		{
			mode = value;
			float size = modeIcon.Texture.GetSize().Y;
			modeIcon.RegionRect = new Rect2((int)value * modeIcon.RegionRect.Size.Y, 0, size, size);
		}
	}




	public enum CursorMode
	{
		Inspect,
		Build,
		Break
	}



	public override void _Ready()
    {
		ownerPlayer = GetParent<Player>();
		grid = GetTree().Root.GetNode<TileMap>($"{GameManager.Instance.CurrentLevel}/Grid");
		highlighter = GetNode<Sprite2D>("Highlighter");
		modeIcon = GetNode<Sprite2D>("ModeIcon");
		gridSize = grid.TileSet.TileSize[0];
		
		Pulse();
    }



    public override void _Input(InputEvent @event)
    {
		if (@event.IsActionPressed("TileInteract") && ownerPlayer.Alive)
			TileInteraction();

		if (@event.IsActionPressed("CurserModeInspect"))
			Mode = CursorMode.Inspect;
			
		if (@event.IsActionPressed("CursorModeBuild"))
			Mode = CursorMode.Build;

		if (@event.IsActionPressed("CursorModeBreak"))
			Mode = CursorMode.Break;
    }



	private void TileInteraction()
	{
		Vector2I placePos = (Vector2I)(mousePos - mousePos % gridSize) / gridSize;

		if (mousePos.X < 0)
			placePos.X -= 1;

		if (mousePos.Y < 0)
			placePos.Y -= 1;

		switch (Mode)
		{
			case CursorMode.Inspect:
				ModeIconAnim();
				break;

			case CursorMode.Build:
				ModeIconAnim();
				if (Input.IsActionPressed("LeftClick"))
				{
					grid.SetCell(1, placePos, 2, new Vector2I(1, 0));
				}

				if (Input.IsActionPressed("RightClick"))
				{
					grid.SetCell(1, placePos, 2);
				}
				break;

			case CursorMode.Break:
				ModeIconAnim();
				break;

			default:
				throw new Exception($"Invalid cursor mode: {Mode}");
		}
	}



	private void ModeIconAnim()
	{
		if (inCursorAnim)
			return;

		inCursorAnim = true;
		Tween swingAnim = CreateTween();

		switch (Mode)
		{
			case CursorMode.Inspect:
				swingAnim.TweenProperty(modeIcon, "rotation_degrees", -90, 0.4).SetTrans(Tween.TransitionType.Circ);
				swingAnim.TweenProperty(modeIcon, "rotation_degrees", 0, 0.4).SetTrans(Tween.TransitionType.Circ);
				break;

			case CursorMode.Build:
			case CursorMode.Break:
				swingAnim.TweenProperty(modeIcon, "rotation_degrees", 90, 0.3).SetTrans(Tween.TransitionType.Elastic).SetEase(Tween.EaseType.Out);
				swingAnim.TweenProperty(modeIcon, "rotation_degrees", 0, 0.2).SetEase(Tween.EaseType.In);
				break;

			default:
				swingAnim.Kill();
				return;
		}
	
		swingAnim.Finished += OnCursorAnimFinished;
	}



	private void OnCursorAnimFinished() => inCursorAnim = false;



    public override void _Process(double delta)
	{
		mousePos = GetGlobalMousePosition();
		SnapToGrid();
	}



	private void SnapToGrid()
	{
		float xPos = mousePos.X > 0 ?
			mousePos.X - (mousePos.X % gridSize) + (gridSize / 2) :
			mousePos.X - (mousePos.X % gridSize) - (gridSize / 2);
		
		float yPos = mousePos.Y > 0 ?
			mousePos.Y - (mousePos.Y % gridSize) + (gridSize / 2) :
			mousePos.Y - (mousePos.Y % gridSize) - (gridSize / 2);

		Position = new Vector2(xPos, yPos);
	}



	private void Pulse()
	{
		Tween tween = CreateTween().SetLoops();
		tween.TweenProperty(highlighter, "modulate:a", 0.1, 1);
		tween.TweenProperty(highlighter, "modulate:a", 0.5, 1);
	}
}
