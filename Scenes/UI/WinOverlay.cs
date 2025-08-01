using Godot;
using System;

public partial class WinOverlay : Control
{
	private Label Ticks => this.GetNode<Label>("Dialog/MarginContainer/VBoxContainer/TickCounter");
	private Control GoldMedalIndicator => this.GetNode<Control>("Dialog/MarginContainer/VBoxContainer/Gold Medal");
	private Control SilverMedalIndicator => this.GetNode<Control>("Dialog/MarginContainer/VBoxContainer/Silver Medal");
	private Control BronceMedalIndicator => this.GetNode<Control>("Dialog/MarginContainer/VBoxContainer/Bronce Medal");
	
	[Signal]
	public delegate void RestartLevelEventHandler();
	
	[Signal]
	public delegate void NextLevelEventHandler();

	public void Open(int tickCount, bool hasGoldMedal, bool hasSilverMedal, bool hasBronceMedal)
	{
		this.Ticks.Text = $"Took {tickCount} Ticks";
		this.GoldMedalIndicator.Visible = hasGoldMedal;
		this.SilverMedalIndicator.Visible = hasSilverMedal;
		this.BronceMedalIndicator.Visible = hasBronceMedal;
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