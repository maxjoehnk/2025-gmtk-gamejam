using Godot;
using System;

public partial class WinOverlay : Control
{
	private Label Title => this.GetNode<Label>("Dialog/MarginContainer/VBoxContainer/Title");
	private Label Ticks => this.GetNode<Label>("Dialog/MarginContainer/VBoxContainer/TickCounter");
	private Control GoldMedalIndicator => this.GetNode<Control>("Dialog/MarginContainer/VBoxContainer/Medals/Gold Medal");
	private Control SilverMedalIndicator => this.GetNode<Control>("Dialog/MarginContainer/VBoxContainer/Medals/Silver Medal");
	private Control BronzeMedalIndicator => this.GetNode<Control>("Dialog/MarginContainer/VBoxContainer/Medals/Bronze Medal");
	
	[Signal]
	public delegate void RestartLevelEventHandler();
	
	[Signal]
	public delegate void NextLevelEventHandler();

	public void Open(string name, int tickCount, bool hasGoldMedal, bool hasSilverMedal, bool hasBronzeMedal)
	{
		this.Title.Text = name;
		this.Ticks.Text = $"Took {tickCount} Ticks";
		this.GoldMedalIndicator.Visible = hasGoldMedal;
		this.SilverMedalIndicator.Visible = hasSilverMedal;
		this.BronzeMedalIndicator.Visible = hasBronzeMedal;
		this.Show();
	}

	public void OnRestartPressed()
	{
		this.EmitSignalRestartLevel();
	}

	public void OnNextPressed()
	{
		this.EmitSignalNextLevel();
	}
}