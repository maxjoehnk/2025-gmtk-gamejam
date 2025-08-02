using gmtkgamejam.Core;
using Godot;
using System;

public partial class PauseMenuOverlay : Control
{
    public override void _Ready()
    {
        this.Visible = false;
    }

    public void Open()
    {
        GD.Print("Game paused");
        this.Visible = true;
        GetTree().Paused = true;
        this.Show();
    }

    private void TogglePause()
    {
        GD.Print($"Toggle Pause");
        GetTree().Paused = true;
    }

    public void OnResumePressed()
    {
        GD.Print($"Toggle unpaused");
        GetTree().Paused = false;
        this.Hide();
    }

    public void OnMainMenuPressed()
    {
        GetTree().Paused = false;
        LevelLoader.Instance.OpenMainMenu();
    }

    public void OnDesktopPressed()
    {
        GetTree().Quit();
    }
}
