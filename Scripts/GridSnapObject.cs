using Godot;

/// <summary>
/// Only used for preview of pplacable things. If something gets placed, it will suummon the real object and remove itself.
/// </summary>
public partial class GridSnapObject : Node2D
{
	private TileMap grid;
	public PackedScene placableObject;



    public override void _Ready()
    {
		grid = GetTree().Root.GetNode<TileMap>($"{GameManager.Instance.CurrentLevel}/Grid");
    }



    public override void _Process(double delta)
	{
		SnapToGrid();
	}



	protected virtual void SnapToGrid()
	{
		Vector2 mousePos = GetGlobalMousePosition();
		int gridSize = grid.TileSet.TileSize[0];

		float xPos = mousePos.X > 0 ?
			mousePos.X - (mousePos.X % gridSize) + (gridSize / 2) :
			mousePos.X - (mousePos.X % gridSize) - (gridSize / 2);
		
		float yPos = mousePos.Y > 0 ?
			mousePos.Y - (mousePos.Y % gridSize) + (gridSize / 2) :
			mousePos.Y - (mousePos.Y % gridSize) - (gridSize / 2);

		GlobalPosition = new Vector2(xPos, yPos);
	}
}
