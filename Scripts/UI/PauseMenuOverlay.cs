using gmtkgamejam.Core;
using Godot;
using System;

public partial class PauseMenuOverlay : Control
{
	public override void _Ready()
	{
		this.Visible = false;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("OpenMenu"))
		{
			this.OnResumePressed();
			this.GetTree().Root.SetInputAsHandled();
		}
	}

	public void Open()
	{
		GD.Print("Game paused");
		this.Visible = true;
		this.GetTree().Paused = true;
		this.Show();
		Music.Instance.TransitionToMenuMusic();
	}

	public void OnResumePressed()
	{
		GD.Print($"Toggle unpaused");
		this.GetTree().Paused = false;
		this.Hide();
		Music.Instance.TransitionToGameMusic();
	}

	public void OnMainMenuPressed()
	{
		this.GetTree().Paused = false;
		LevelManager.Instance.OpenMainMenu();
	}

	public void OnSettingsPressed()
	{
		this.GetTree().Paused = false;
		LevelManager.Instance.OpenSettings();
	}

	public void OnDesktopPressed()
	{
		this.GetTree().Quit();
	}
}