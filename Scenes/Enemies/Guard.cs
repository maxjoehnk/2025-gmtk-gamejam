using System;
using gmtkgamejam.Scenes;
using Godot;

public partial class Guard : PathFollow2D
{
	private ActionPlayer Playback => GetNode<ActionPlayer>("/root/Game/ActionPlayer");

	[Export] public float Speed { get; set; } = 128;

	[Signal]
	public delegate void CatchedPlayerEventHandler();

	private CharacterBody2D Character => GetNode<CharacterBody2D>("CharacterBody2D");

	public override void _Ready()
	{
		Playback.Ticked += OnTick;
	}

	private void OnTick(int tick)
	{
		this.Progress = this.Speed * tick;
	}

	public void OnHitPlayer(Node2D player)
	{
		GD.Print("Hit player" + player);
		this.EmitSignalCatchedPlayer();
	}
}