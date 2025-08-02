using System;
using Godot;
using gmtkgamejam.Scripts.Core;

public partial class PlaybackToolbar : HBoxContainer, IClocked
{
	private Label TickLabel => this.GetNode<Label>("Label");

	private TextureButton NormalSpeedButton => this.GetNode<TextureButton>("Speed/SpeedNormal");
	private TextureButton FastSpeedButton => this.GetNode<TextureButton>("Speed/SpeedFast");
	private TextureButton FastestSpeedButton => this.GetNode<TextureButton>("Speed/SpeedFastest");

	public void OnTick(int tick)
	{
		this.TickLabel.Text = tick.ToString();
	}

	public void SetSpeed(float speed)
	{
		this.NormalSpeedButton.SetPressed(Math.Abs(speed - 1) < float.Epsilon); 
		this.FastSpeedButton.SetPressed(Math.Abs(speed - 4) < float.Epsilon);
		this.FastestSpeedButton.SetPressed(Math.Abs(speed - 8) < float.Epsilon);
	}
}