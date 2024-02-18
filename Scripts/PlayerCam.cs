using Godot;

public partial class PlayerCam : Camera2D
{
	private Player player;
	public Player Player
	{
		private get => player;
		set
		{
			if (player == null)
				player = value;
			else
				GD.PrintErr("PlayerCam cannot be assinged a second time!");
		}
	}



	public override void _Ready()
	{
	}



	public override void _Process(double delta)
	{
	}
}
