using System;
using Godot;

public partial class PreviewCursor : Node2D
{
	private Player ownerPlayer;
	private TileMap grid;
	private Sprite2D highlighter;
	private Sprite2D modeIcon;
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
			float s = GameManager.Instance.SingleTileSize;
			modeIcon.RegionRect = new Rect2((int)value * s, 0, s, s);
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
		gridSize = GameManager.Instance.SingleTileSize;

		Pulse();
    }



	public override void _Process(double delta)
	{
		mousePos = GetGlobalMousePosition();
		SnapToGrid();
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
		Vector2I clickPos = (Vector2I)(mousePos - mousePos % gridSize) / gridSize;

		if (mousePos.X < 0)
			clickPos.X -= 1;

		if (mousePos.Y < 0)
			clickPos.Y -= 1;

		switch (Mode)
		{
			case CursorMode.Inspect:
				ModeIconAnim();
				break;

			case CursorMode.Build:
				ModeIconAnim();
				ItemData item = ownerPlayer.Inventory.Items[ownerPlayer.Inventory.Hotbar.Selected];

				if (Input.IsActionPressed("LeftClick"))
				{
					if (item == null || !(item is ItemPlacableData))
						return;
					
					PlaceBlock(item as ItemPlacableData, clickPos);
				}

				if (Input.IsActionPressed("RightClick"))
					BreakBlock(clickPos);
					
				break;

			case CursorMode.Break:
				ModeIconAnim();
				break;

			default:
				throw new Exception($"Invalid cursor mode: {Mode}");
		}
	}



	private void PlaceBlock(ItemPlacableData item, Vector2I position)
	{
		if (grid.GetCellSourceId(item.PlaceLayer, position) != -1)
			return;
		
		Godot.Collections.Array<Vector2I> surroundingCells = grid.GetSurroundingCells(position);
		int neighbourCount = 0;

		foreach (Vector2I neighbour in surroundingCells)
		{
			if (grid.GetCellSourceId(item.PlaceLayer, neighbour) != -1)
				neighbourCount ++;
		}

		if (neighbourCount == 0)
			return;

		grid.SetCell(item.PlaceLayer, position, item.PlaceLayer, item.TileCoordinates);
		item.StackSize --;
	}



	private void BreakBlock(Vector2I position)
	{
		int layerCount = grid.GetLayersCount();

		for (int i = layerCount - 1; i >= 0; i--)
		{
			if (grid.GetCellSourceId(i, position) == -1)
				continue;

			ItemPlacableData item = grid.GetCellTileData(i, position).GetCustomData("ItemData").AsGodotObject() as ItemPlacableData;
			ownerPlayer.Inventory.AddItem(item.Duplicate(true) as ItemPlacableData);
			grid.EraseCell(i, position);
			return;
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



	private void SnapToGrid()
	{
		float xPos = mousePos.X > 0 ?
			mousePos.X - (mousePos.X % gridSize) + (gridSize / 2) :
			mousePos.X - (mousePos.X % gridSize) - (gridSize / 2);
		
		float yPos = mousePos.Y > 0 ?
			mousePos.Y - (mousePos.Y % gridSize) + (gridSize / 2) :
			mousePos.Y - (mousePos.Y % gridSize) - (gridSize / 2);

		Vector2 newPosition = new Vector2(xPos, yPos);

		if (Position != newPosition)
		{
			Position = newPosition;

			if (Input.IsActionPressed("TileInteract") && ownerPlayer.Alive)
				TileInteraction();
		} 
	}



	private void Pulse()
	{
		Tween tween = CreateTween().SetLoops();
		tween.TweenProperty(highlighter, "modulate:a", 0.1, 1);
		tween.TweenProperty(highlighter, "modulate:a", 0.5, 1);
	}
}
