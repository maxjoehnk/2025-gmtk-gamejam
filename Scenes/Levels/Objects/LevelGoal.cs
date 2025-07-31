using Godot;
using System;

public partial class LevelGoal : Node2D
{
	[Signal]
	public delegate void PlayerReachedGoalEventHandler();

	public override void _Ready()
	{
		GD.Print("LevelGoal Ready");
	}

	public void OnPlayerEntered(Node2D player)
	{
		GD.Print("Player " + player + " entered goal");
		this.EmitSignalPlayerReachedGoal();
	}
}
