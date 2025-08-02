using Godot;
using System;
using System.Globalization;
using gmtkgamejam.Scenes;
using gmtkgamejam.Scripts.Core;

public partial class Spikes : Node2D, ISwitchable, IResettable
{
	private StaticBody2D Collider => this.GetNode<StaticBody2D>("Spikes");
	private AnimatedSprite2D Animation => this.GetNode<AnimatedSprite2D>("Spikes/Spikes");

	[Export] public bool IsActive { get; set; } = true;
	
	private bool CurrentAnimationState => this.Animation.Animation == "Up";

	public bool State { get; set; } = true;

	public override void _Process(double delta)
	{
		this.Collider.ProcessMode = this.State ? ProcessModeEnum.Inherit : ProcessModeEnum.Disabled;
		this.Animation.SpeedScale = (float)ActionPlayer.Instance.PlaybackSpeed;
		if (this.State != this.CurrentAnimationState)
		{
			this.Animation.Play(this.State ? "Up" : "Down");
		}
	}

	public override void _Ready()
	{
		this.Reset();
	}

	public void Toggle()
	{
		this.State = !this.State;
	}

	public void Reset()
	{
		this.State = this.IsActive;
		this.Animation.Play(this.State ? "Up" : "Down");
	}
}