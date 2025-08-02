using Godot;

public partial class PauseMenu : Control
{
    [Export] public Button ResumeButton;
    [Export] public Button MainMenuButton;
    [Export] public Button ExitButton;

    public override void _Ready()
    {
        Visible = false;
        ProcessMode = ProcessModeEnum.Always;

        ResumeButton.Pressed += OnResumePressed;
        MainMenuButton.Pressed += OnMainMenuPressed;
        ExitButton.Pressed += OnExitPressed;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("OpenMenu"))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        GetTree().Paused = !GetTree().Paused;
        Visible = GetTree().Paused;
    }

    private void OnResumePressed()
    {
        GetTree().Paused = false;
        Visible = false;
    }

    private void OnMainMenuPressed()
    {
        GetTree().Paused = false;
        Visible = false;
        GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
    }

    private void OnExitPressed()
    {
        GetTree().Paused = false;
        GetTree().Quit();
    }
}
