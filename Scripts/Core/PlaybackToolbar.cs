using System;
using Godot;
using gmtkgamejam.Scripts.Core;

public partial class PlaybackToolbar : HBoxContainer, IClocked
{
	private Label TickLabel => this.GetNode<Label>("Label");

	private TextureButton NormalSpeedButton => this.GetNode<TextureButton>("SpeedNormal");
	private TextureButton FastSpeedButton => this.GetNode<TextureButton>("SpeedFast");
	private TextureButton FastestSpeedButton => this.GetNode<TextureButton>("SpeedFastest");

	[Signal]
	public delegate void SpeedChangedEventHandler(float speed);
	
	public override void _Ready()
	{
		this.NormalSpeedButton.Pressed += this.OnNormalSpeedPressed;
		this.FastSpeedButton.Pressed += this.OnFastSpeedPressed;
		this.FastestSpeedButton.Pressed += this.OnFastestSpeedPressed;
	}
	
	public void OnTick(int tick)
	{
		this.TickLabel.Text = tick.ToString("00");
		if (tick > 99)
		{
			this.TickLabel.Text = "99";
		}
	}
	
	public void OnNormalSpeedPressed()
	{
		this.EmitSignalSpeedChanged(1);
	}
	
	public void OnFastSpeedPressed()
	{
		this.EmitSignalSpeedChanged(4);
	}
	
	public void OnFastestSpeedPressed()
	{
		this.EmitSignalSpeedChanged(8);
	}

	public void SetSpeed(float speed)
	{
		this.NormalSpeedButton.SetPressed(Math.Abs(speed - 1) < float.Epsilon); 
		this.FastSpeedButton.SetPressed(Math.Abs(speed - 4) < float.Epsilon);
		this.FastestSpeedButton.SetPressed(Math.Abs(speed - 8) < float.Epsilon);
	}
}