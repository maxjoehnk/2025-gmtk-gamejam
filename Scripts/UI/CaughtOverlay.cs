using Godot;
using System;
using gmtkgamejam.Core;

public partial class CaughtOverlay : Control
{
	private Label Title => this.GetNode<Label>("Dialog/MarginContainer/VBoxContainer/Title Container/Title");
	
	[Signal]
	public delegate void RestartLevelEventHandler();

	public void Open()
	{
		this.Title.Text = LevelManager.Instance.CurrentLevel?.Name;
		this.Show();
	}

	public void OnRestartPressed()
	{
		this.EmitSignalRestartLevel();
	}

	public void OnExitPressed()
	{
		LevelManager.Instance.OpenMainMenu();
	}
}