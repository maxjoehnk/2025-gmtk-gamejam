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
		this.GetTree().Paused = true;
		this.Show();
		Music.Instance.TransitionToMenuMusic();
	}

	private void TogglePause()
	{
		GD.Print($"Toggle Pause");
		this.GetTree().Paused = true;
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

	public void OnDesktopPressed()
	{
		this.GetTree().Quit();
	}
}