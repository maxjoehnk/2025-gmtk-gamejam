using Godot;
using System;
using gmtkgamejam.Scripts.Objects;

public partial class LevelGoal : Node2D
{
	private Node2D Mannhole => this.GetNode<Node2D>("LevelExitManhole");
	private Node2D Ladder => this.GetNode<Node2D>("LevelExitLadder");
	private Node2D Pond => this.GetNode<Node2D>("LevelExitPond");
	
	[Export]
	public LevelExitStyle ExitStyle { get; set; } = LevelExitStyle.Mannhole;
	
	[Signal]
	public delegate void PlayerReachedGoalEventHandler();

	public override void _Ready()
	{
		this.UpdateSprite();
	}

	public override void _Process(double delta)
	{
		this.UpdateSprite();
	}

	public void OnPlayerEntered(Node2D player)
	{
		GD.Print("Player " + player + " entered goal");
		this.EmitSignalPlayerReachedGoal();
	}

	private void UpdateSprite()
	{
		this.Mannhole.Visible = this.ExitStyle == LevelExitStyle.Mannhole;
		this.Ladder.Visible = this.ExitStyle == LevelExitStyle.Ladder;
		this.Pond.Visible = this.ExitStyle == LevelExitStyle.Pond;
	}
}