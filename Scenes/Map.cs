using Godot;
using System;
using gmtkgamejam.Scenes.Enemies;
using Godot.Collections;

public partial class Map : Node
{
	public Vector2 SpawnPosition => this.GetNode<Node2D>("TileMapLayer/Spawn").GlobalPosition;
	public Game Game => this.GetNode<Game>("Game");

	[Export] public int GoldMedalTicks { get; set; }

	[Export] public int SilverMedalTicks { get; set; }

	[Export] public int BronzeMedalTicks { get; set; }
	
	[Signal]
	public delegate void PlayerLostEventHandler();
	
	[Signal]
	public delegate void PlayerWonEventHandler();

	public override void _Ready()
	{
		Array<Node> enemies = this.GetTree().GetNodesInGroup("Enemies");
		foreach (Node enemy in enemies)
		{
			GD.Print($"Connecting catched player signal from {enemy} ({enemy.Name})");
			enemy.Connect(nameof(Enemy.CatchedPlayer), Callable.From(this.OnPlayerCaught));
		}

		this.Game.SpawnPosition = this.SpawnPosition;
		this.Game.Respawn();
	}

	private void OnPlayerCaught()
	{
		GD.Print("Player caught");
		this.Game.OnPlayerLost();
	}

	public void OnPlayerWon()
	{
		GD.Print("Player won");
		this.Game.OnPlayerWon(this.GoldMedalTicks, this.SilverMedalTicks, this.BronzeMedalTicks);
	}
}
