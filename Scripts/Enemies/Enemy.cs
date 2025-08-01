using Godot;

namespace gmtkgamejam.Scenes.Enemies;

public partial class Enemy : Node2D
{
	[Signal]
	public delegate void CatchedPlayerEventHandler();
	
	public void OnHitPlayer(Node2D player)
	{
		GD.Print($"Camera Hit player {player} ({player.Name})");
		this.EmitSignalCatchedPlayer();
	}
}