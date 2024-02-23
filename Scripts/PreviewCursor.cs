using System;
using Godot;

public partial class PreviewCursor : Node2D
{
	private TileMap grid;
	private Sprite2D sprite;
	public PackedScene placableObject;
	private Vector2 mousePos;
	private int gridSize;
	public CursorMode Mode { get; set; } = CursorMode.Inspect;




	public enum CursorMode
	{
		Inspect,
		Build,
		Destroy,
		Break
	}

	private void CursorToInspect() => Mode = CursorMode.Inspect;
	private void CursorToBuild() => Mode = CursorMode.Build;
	private void CursorToDetroy() => Mode = CursorMode.Destroy;
	private void CursorToBreak() => Mode = CursorMode.Break;



	public override void _Ready()
    {
		grid = GetTree().Root.GetNode<TileMap>($"{GameManager.Instance.CurrentLevel}/Grid");
		sprite = GetNode<Sprite2D>("Sprite");
		gridSize = grid.TileSet.TileSize[0];
		
		Pulse();
    }



    public override void _Input(InputEvent @event)
    {
		if (@event.IsActionPressed("TileInteract"))
		{
			Vector2I placePos = (Vector2I)(mousePos - mousePos % grid.TileSet.TileSize[0]) / 16;

			if (mousePos.X < 0)
				placePos.X -= 1;

			if (mousePos.Y < 0)
				placePos.Y -= 1;

			switch (Mode)
			{
				case CursorMode.Inspect:
					break;

				case CursorMode.Build:
					grid.SetCell(1, placePos, 2, new Vector2I(1, 0));
					break;

				case CursorMode.Destroy:
					grid.SetCell(1, placePos, 2);
					break;

				case CursorMode.Break:
					break;
				
				default:
					throw new Exception($"Invalid cursor mode: {Mode}");
			}
		}
    }



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
		tween.TweenProperty(sprite, "modulate:a", 0.1, 1);
		tween.TweenProperty(sprite, "modulate:a", 0.5, 1);
	}
}
