using Godot;
using System;
using Godot.Collections;

public partial class Map : TileMapLayer
{
	public Vector2 SpawnPosition => GetNode<Node2D>("Spawn").GlobalPosition;
	
	[Signal]
	public delegate void PlayerLostEventHandler();
	
	[Signal]
	public delegate void PlayerWonEventHandler();

	public override void _Ready()
	{
		Array<Node> enemies = this.GetTree().GetNodesInGroup("Enemies");
		foreach (Node enemy in enemies)
		{
			enemy.Connect(nameof(Guard.CatchedPlayer), Callable.From(this.OnPlayerCaught));
		}
	}

	private void OnPlayerCaught()
	{
		this.EmitSignalPlayerLost();
	}
}
