using Godot;
using System;
using gmtkgamejam.Scenes;
using gmtkgamejam.Scripts.Core;
using Godot.Collections;

public partial class CountdownSwitch : Node2D, IInteractable, IResettable, IClocked
{
	private Label CountdownLabel => this.GetNode<Label>("Countdown");
	private AnimatedSprite2D Switch => this.GetNode<AnimatedSprite2D>("Switch");
	
	[Export] public Array<NodePath> Targets { get; set; } = new();

	[Export] public int Countdown { get; set; } = 3;
	
	private bool active;
	private int ticksRemaining;

	public override void _Ready()
	{
		this.ticksRemaining = this.Countdown;
	}

	public override void _Process(double delta)
	{
		this.Switch.SpeedScale = (float)ActionPlayer.Instance.PlaybackSpeed;
		this.CountdownLabel.Text = this.ticksRemaining.ToString();
		this.CountdownLabel.Visible = this.active;
	}

	public void Interact(Player player)
	{
		this.ToggleSwitches();
		this.Switch.Play();
		this.active = true;
		this.ticksRemaining = this.Countdown;
	}

	private void ToggleSwitches()
	{
		foreach (NodePath nodePath in Targets)
		{
			ISwitchable switchable = GetNode<ISwitchable>(nodePath);
			switchable.Toggle();
		}
	}

	public void OnTick(int tick)
	{
		if (!this.active)
		{
			return;
		}

		this.ticksRemaining--;
		if (this.ticksRemaining <= 0)
		{
			this.Reset();
			this.Switch.PlayBackwards();
			ToggleSwitches();
		}
	}

	public void Reset()
	{
		GD.Print("CountdownSwitch reset");
		this.ticksRemaining = this.Countdown;
		this.active = false;
	}
}
