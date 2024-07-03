using Godot;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    public string CurrentLevel { get; set; }
    public int SingleTileSize { get; } = 16;
    private readonly CompressedTexture2D inWorldCursor = GD.Load<CompressedTexture2D>("res://Assets/Player/InWorldCursor.png");



    public override void _Ready()
    {
        /// TEMP \/ Temporary values

        CurrentLevel = "DevWorld";

        /// TEMP /\

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



    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionReleased("ToggleFullscreen"))
        {
            GetWindow().Mode = GetWindow().Mode == Window.ModeEnum.Windowed ? Window.ModeEnum.Fullscreen : Window.ModeEnum.Windowed;
        }
    }



    public static void ShowInWorldCursor(bool yes)
    {
        if (yes)
            Input.SetCustomMouseCursor(Instance.inWorldCursor, hotspot: Instance.inWorldCursor.GetSize() / 2);
        else 
            Input.SetCustomMouseCursor(null);
    }
}
