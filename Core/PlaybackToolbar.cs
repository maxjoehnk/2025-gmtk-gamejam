using Godot;
using System;

public partial class PlaybackToolbar : HBoxContainer
{
	private Label TickLabel => GetNode<Label>("Label");

	public void OnTick(int tick)
	{
		TickLabel.Text = "Tick: " + tick;
	}
}
