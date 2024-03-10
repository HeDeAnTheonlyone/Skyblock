using Godot;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    public string CurrentLevel { get; set; }
    public Player player;
    public int SingleTileSize { get; } = 16;



    public override void _Ready()
    {
        /// TODO Remove Temporary values

        CurrentLevel = "DevWorld";

        ///

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
