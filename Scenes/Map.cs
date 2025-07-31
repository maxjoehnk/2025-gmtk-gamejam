using Godot;
using System;
using Godot.Collections;

public partial class Map : Node
{
	public Vector2 SpawnPosition => this.GetNode<Node2D>("TileMapLayer/Spawn").GlobalPosition;
	public Game Game => this.GetNode<Game>("Game");
	
	[Signal]
	public delegate void PlayerLostEventHandler();
	
	[Signal]
	public delegate void PlayerWonEventHandler();

	public override void _Ready()
	{
		Array<Node> enemies = this.GetTree().GetNodesInGroup("Enemies");
		foreach (Node enemy in enemies)
		{
			GD.Print("Connecting catched player signal from " + enemy);
			enemy.Connect(nameof(Guard.CatchedPlayer), Callable.From(this.OnPlayerCaught));
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
		this.Game.OnPlayerWon();
	}
}
