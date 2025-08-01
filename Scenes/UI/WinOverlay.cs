using Godot;
using System;
using gmtkgamejam.Core;

public partial class WinOverlay : Control
{
	private Label Title => this.GetNode<Label>("Dialog/MarginContainer/VBoxContainer/Title Container/Title");
	private Label Ticks => this.GetNode<Label>("Dialog/MarginContainer/VBoxContainer/HBoxContainer2/VBoxContainer/TickCounter");
	private Control GoldMedalIndicator => this.GetNode<Control>("Dialog/MarginContainer/VBoxContainer/HBoxContainer2/Medals/Gold Medal");
	private Control SilverMedalIndicator => this.GetNode<Control>("Dialog/MarginContainer/VBoxContainer/HBoxContainer2/Medals/Silver Medal");
	private Control BronzeMedalIndicator => this.GetNode<Control>("Dialog/MarginContainer/VBoxContainer/HBoxContainer2/Medals/Bronze Medal");

	private Control NextLevelButton =>
		this.GetNode<Control>("Dialog/MarginContainer/VBoxContainer/HBoxContainer/Next Level");
	
	[Signal]
	public delegate void RestartLevelEventHandler();

	public void Open(string name, int tickCount, bool hasGoldMedal, bool hasSilverMedal, bool hasBronzeMedal)
	{
		this.Title.Text = name;
		this.Ticks.Text = $"{tickCount} Ticks";
		this.GoldMedalIndicator.Visible = hasGoldMedal;
		this.SilverMedalIndicator.Visible = hasSilverMedal;
		this.BronzeMedalIndicator.Visible = hasBronzeMedal;
		this.NextLevelButton.Visible = LevelLoader.Instance.HasNextLevel();
		this.Show();
	}

	public void OnRestartPressed()
	{
		this.EmitSignalRestartLevel();
	}

	public void OnNextPressed()
	{
		LevelLoader.Instance.LoadNextLevel();
	}

	public void OnExitPressed()
	{
		LevelLoader.Instance.OpenMainMenu();
	}
}