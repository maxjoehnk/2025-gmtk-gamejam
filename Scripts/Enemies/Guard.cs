using System;
using gmtkgamejam.Scenes;
using Godot;

public partial class Guard : PathFollow2D
{
	private ActionPlayer Playback => ActionPlayer.Get(this);

	[Export] public float Speed { get; set; } = 128;

	[Export] public int StartPosition { get; set; } = 0;

	[Signal]
	public delegate void CatchedPlayerEventHandler();

	private CharacterBody2D Character => GetNode<CharacterBody2D>("CharacterBody2D");

	private int targetPoint = 0;
	private int lastPoint = 0;
	private double currentSubTime = 0;

	public override void _Ready()
	{
		Playback.Ticked += OnTick;

		this.targetPoint = this.StartPosition;
		this.lastPoint = this.StartPosition;
		this.currentSubTime = 0;
		this.Progress = this.StartPosition;
	}

	private void OnTick(int tick)
	{
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
		if (currentSubTime > this.Playback.TickDuration)
		{
			this.lastPoint = Math.Min(this.lastPoint + 1, this.targetPoint);
			this.currentSubTime -= this.Playback.TickDuration;
		}

		if (this.lastPoint == this.targetPoint)
			this.currentSubTime = 0;
		double ratio =
			Mathf.Sin(-Mathf.Pi * 0.5 + Mathf.Pi * Mathf.Min(1, this.currentSubTime / this.Playback.TickDuration)) * 0.5 +
			0.5;
		double interpolatedPoint = this.lastPoint * (1 - ratio) + Math.Min(this.lastPoint + 1, this.targetPoint) * ratio;
		this.Progress = this.Speed * (float)interpolatedPoint;
	}
}