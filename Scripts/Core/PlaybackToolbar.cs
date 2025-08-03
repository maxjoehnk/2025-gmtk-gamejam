using System;
using Godot;
using gmtkgamejam.Scripts.Core;

public partial class PlaybackToolbar : HBoxContainer, IClocked
{
	private Label TickLabel => this.GetNode<Label>("Label");

	private TextureButton NormalSpeedButton => this.GetNode<TextureButton>("View/PlaybackToolbarCanvas/VBoxContainer/PlaybackToolbar/SpeedNormal");
	private TextureButton FastSpeedButton => this.GetNode<TextureButton>("View/PlaybackToolbarCanvas/VBoxContainer/PlaybackToolbar/SpeedFast");
	private TextureButton FastestSpeedButton => this.GetNode<TextureButton>("View/PlaybackToolbarCanvas/VBoxContainer/PlaybackToolbar/SpeedFastest");

	public void OnTick(int tick)
	{
		this.TickLabel.Text = tick.ToString();
		if(tick < 10)
		{
			this.TickLabel.Text = "0" + tick.ToString();
		}
		if (tick > 99)
		{
			this.TickLabel.Text = "99";
		}
	}

	public void SetSpeed(float speed)
	{
		this.NormalSpeedButton.SetPressed(Math.Abs(speed - 1) < float.Epsilon); 
		this.FastSpeedButton.SetPressed(Math.Abs(speed - 4) < float.Epsilon);
		this.FastestSpeedButton.SetPressed(Math.Abs(speed - 8) < float.Epsilon);
	}
}