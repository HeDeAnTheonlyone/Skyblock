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
	private Tween swingAnim;
	private CursorMode mode = CursorMode.Interact;
	public CursorMode Mode
	{
		get => mode;
		set
		{
			mode = value;
			float tileSize = GameManager.Instance.SingleTileSize;
			modeIcon.RegionRect = new Rect2((int)value * tileSize, 0, tileSize, tileSize);
		}
	}



	public enum CursorMode
	{
		Interact,
		Build,
		Break,
		Inspect
	}



	public override void _Ready()
    {
		ownerPlayer = GetParent<Player>();
		grid = GetTree().Root.GetNode<TileMap>($"{GameManager.Instance.CurrentLevel}/Grid");
		highlighter = GetNode<Sprite2D>("Highlighter");
		modeIcon = GetNode<Sprite2D>("ModeIcon");
		gridSize = GameManager.Instance.SingleTileSize;

		Pulse();

		CallDeferred("ConnectSignal"); 
    }



	private void ConnectSignal()
	{
		ownerPlayer.Inventory.InventoryChanged += UpdateCursorMode;
		ownerPlayer.Inventory.Hotbar.HotbarSlotChanged += UpdateCursorMode;
	}



    public override void _ExitTree()
	{
		ownerPlayer.Inventory.InventoryChanged -= UpdateCursorMode;
		ownerPlayer.Inventory.Hotbar.HotbarSlotChanged -= UpdateCursorMode;
	}



    public override void _Process(double delta)
	{
		mousePos = GetGlobalMousePosition();
		if (!SnapToGrid()) return;

		// Makes it possible to hold down the button to keep placing blocks
		if (Input.IsActionPressed("TileInteract") && ownerPlayer.Alive) TileInteraction();
	}



	public override void _Input(InputEvent @event)
    {
		if (@event.IsActionPressed("TileInteract") && ownerPlayer.Alive) TileInteraction();
    }



	private void UpdateCursorMode() => UpdateCursorMode(ownerPlayer.Inventory.Hotbar.Selected);



	private void UpdateCursorMode(int itemIndex)
	{
		CursorMode newMode = ownerPlayer.Inventory.Items[itemIndex] switch
		{
			ItemPlacableData => CursorMode.Build,
			ItemToolData => CursorMode.Break,
			_ => CursorMode.Interact
		};

		if (Mode != newMode)
		{
			if (inCursorAnim) EndTweenEarly();
			Mode = newMode;
		}
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
			case CursorMode.Interact:
			{
				ModeIconAnim();
				break;
			}

			case CursorMode.Build:
			{
				ModeIconAnim();
				ItemData item = ownerPlayer.Inventory.Items[ownerPlayer.Inventory.Hotbar.Selected];

				if (Input.IsActionPressed("LeftClick"))
				{
					if (item == null || !(item is ItemPlacableData)) return;
					
					PlaceBlock(item as ItemPlacableData, clickPos);
				}

				if (Input.IsActionPressed("RightClick")) BreakBlock(clickPos);
				break;
			}

			case CursorMode.Break:
			{
				ModeIconAnim();
				break;
			}

			default:
			{
				throw new Exception($"Invalid cursor mode: {Mode}");
			}
		}
	}



	private void PlaceBlock(ItemPlacableData block, Vector2I position)
	{
		switch (block.Layer)
		{
			case ItemPlacableData.PlaceLayer.Floor:
			{
				if (grid.GetCellSourceId(0, position) != -1) return;
				Godot.Collections.Array<Vector2I> surroundingCells = grid.GetSurroundingCells(position);
				bool hasNeighbor = false;

				foreach (Vector2I neighbor in surroundingCells)
				{
					if (grid.GetCellSourceId(0, neighbor) != -1)
					{
						hasNeighbor = true;
						break;
					}
				}

				if (!hasNeighbor) return;
				break;
			}	

			case ItemPlacableData.PlaceLayer.Decoration:
			{
				if (grid.GetCellSourceId(1, position) != -1 || grid.GetCellSourceId(0, position) == -1) return;
				break;
			}
		}

		grid.SetCell((int)block.Layer, position, (int)block.Layer, block.TileCoordinates);
		block.StackSize --;
	}



	private void BreakBlock(Vector2I position)
	{
		int layerCount = grid.GetLayersCount();

		for (int i = layerCount - 1; i >= 0; i--)
		{
			if (grid.GetCellSourceId(i, position) == -1)
				continue;

			ItemPlacableData item = grid.GetCellTileData(i, position).GetCustomData("ItemData").AsGodotObject() as ItemPlacableData;
			if (item != null)
			{
				ownerPlayer.Inventory.AddItem(item.Duplicate(true) as ItemPlacableData);
			}

			grid.EraseCell(i, position);
			return;
		}
	}



	private void ModeIconAnim()
	{
		if (inCursorAnim)
			return;

		inCursorAnim = true;
		swingAnim = CreateTween();

		switch (Mode)
		{
			case CursorMode.Interact:
			{
				swingAnim.TweenProperty(modeIcon, "rotation_degrees", 90, 0.1).SetTrans(Tween.TransitionType.Circ);
				swingAnim.TweenProperty(modeIcon, "rotation_degrees", 0, 0.3).SetTrans(Tween.TransitionType.Circ);
				break;
			}

			case CursorMode.Build:
			case CursorMode.Break:
			{
				swingAnim.TweenProperty(modeIcon, "rotation_degrees", 90, 0.3).SetTrans(Tween.TransitionType.Elastic).SetEase(Tween.EaseType.Out);
				swingAnim.TweenProperty(modeIcon, "rotation_degrees", 0, 0.2).SetEase(Tween.EaseType.In);
				break;
			}

			default:
			{
				EndTweenEarly();
				return;
			}
		}
	
		swingAnim.Finished += OnCursorAnimFinished;
	}



	private void EndTweenEarly()
	{
		swingAnim.Kill();
		inCursorAnim = false;
		modeIcon.Rotation = 0;
	}



	private void OnCursorAnimFinished() => inCursorAnim = false;



	/// <summary>
	/// Snaps the cursor to the nearest grid field and returns true if it moved to a new grid
	/// </summary>
	/// <returns>true if it moved to a different grid field, otherwise false</returns>
	private bool SnapToGrid()
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
			return true;
		}
		else return false;
	}



	private void Pulse()
	{
		Tween tween = CreateTween().SetLoops();
		tween.TweenProperty(highlighter, "modulate:a", 0.1, 1);
		tween.TweenProperty(highlighter, "modulate:a", 0.5, 1);
	}
}
