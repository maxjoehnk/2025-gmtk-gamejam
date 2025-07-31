using Godot;

public partial class Guard : Node2D
{
	[Export] public float Speed { get; set; } = 128;

	[Signal]
	public delegate void CatchedPlayerEventHandler();
	
	private CharacterBody2D Character => GetNode<CharacterBody2D>("CharacterBody2D");

	public void OnHitPlayer(Node2D player)
	{
		GD.Print("Hit player" + player);
	}
}