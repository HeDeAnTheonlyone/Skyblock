using Godot;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    public string CurrentLevel { get; set; }
    public int SingleTileSize { get; } = 16;
    public int TileOffSet { get; } = 1;
    private readonly CompressedTexture2D inWorldCursor = GD.Load<CompressedTexture2D>("res://Assets/Player/InWorldCursor.png");



    public override void _Ready()
    {
        /// TEMP Remove Temporary values

        CurrentLevel = "DevWorld";

        ///

        Engine.MaxFps = (int)DisplayServer.ScreenGetRefreshRate(DisplayServer.GetPrimaryScreen()) + 20;

        if (Instance == null)
        {
            Instance = this;
            ProcessMode = ProcessModeEnum.Always;
            SetProcess(false);
            SetPhysicsProcess(false);
        }
        else
            QueueFree();

        ShowInWorldCursor(true);
    }



    public static void ShowInWorldCursor(bool yes)
    {
        if (yes)
            Input.SetCustomMouseCursor(Instance.inWorldCursor, hotspot: Instance.inWorldCursor.GetSize() / 2);
        else
            Input.SetCustomMouseCursor(null);
    }
}
