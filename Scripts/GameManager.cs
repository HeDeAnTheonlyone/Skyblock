using Godot;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    public string CurrentLevel { get; set; }



    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
            ProcessMode = ProcessModeEnum.Always;
            SetProcess(false);
            SetPhysicsProcess(false);
        }
        else
            QueueFree();



        /// Temporary values

        CurrentLevel = "DevWorld";

        ///
    }
}
