using Godot;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    public string CurrentLevel { get; set; }
    public int SingleTileSize { get; } = 16;
    public int TileOffSet { get; } = 1;
    


    public override void _Ready()
    {
        /// TEMP Remove Temporary values

        //

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

        Input.MouseMode = Input.MouseModeEnum.Hidden;
    }
}
