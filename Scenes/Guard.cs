using Godot;

public partial class Guard : Node2D
{
	[Export] public float Speed { get; set; } = 128;

	private CharacterBody2D Character => GetNode<CharacterBody2D>("CharacterBody2D");
}