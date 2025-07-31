using Godot;
using System;

public partial class Map : TileMapLayer
{
	public Vector2 SpawnPosition => GetNode<Node2D>("Spawn").GlobalPosition;
}
