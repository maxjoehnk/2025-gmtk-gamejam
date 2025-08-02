using System;
using gmtkgamejam.Scenes;
using gmtkgamejam.Scripts.Core;
using Godot;

public partial class Guard : PathFollow2D, IClocked
{
	[Export] public float Speed { get; set; } = 128;

	[Export] public int StartPosition { get; set; } = 0;

	[Signal]
	public delegate void CatchedPlayerEventHandler();

	private CharacterBody2D Character => GetNode<CharacterBody2D>("CharacterBody2D");

	private int targetPoint;
	private int lastPoint;
	private double currentSubTime;

	public override void _Ready()
	{
		GD.Print($"Guard {this.Name} subscribed to ticks");

		this.targetPoint = this.StartPosition;
		this.lastPoint = this.StartPosition;
		this.currentSubTime = 0;
		this.Progress = this.StartPosition;
	}

	public void OnTick(int tick)
	{
		GD.Print($"Guard {this.Name} ticked {tick}");
		this.targetPoint = this.StartPosition + tick; // replace with waittime
		if (tick == 0)
		{
			this.targetPoint = this.StartPosition;
			this.lastPoint = this.StartPosition;
			this.currentSubTime = this.StartPosition;
			this.Progress = this.StartPosition;
		}
	}

	public void OnHitPlayer(Node2D player)
	{
		GD.Print($"Hit object {player} ({player.Name})");
		if (player is Player)
		{
			this.EmitSignalCatchedPlayer();
		}
	}

	public override void _Process(double delta)
	{
		this.currentSubTime += delta;
		if (this.currentSubTime > ActionPlayer.Instance.TickDuration)
		{
			this.lastPoint = Math.Min(this.lastPoint + 1, this.targetPoint);
			this.currentSubTime -= ActionPlayer.Instance.TickDuration;
		}

		if (this.lastPoint == this.targetPoint)
			this.currentSubTime = 0;
		double ratio =
			Mathf.Sin(-Mathf.Pi * 0.5 + Mathf.Pi * Mathf.Min(1, this.currentSubTime / ActionPlayer.Instance.TickDuration)) * 0.5 +
			0.5;
		double interpolatedPoint = this.lastPoint * (1 - ratio) + Math.Min(this.lastPoint + 1, this.targetPoint) * ratio;
		this.Progress = this.Speed * (float)interpolatedPoint;
	}
}