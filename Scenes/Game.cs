using gmtkgamejam.Scenes;
using Godot;
using Godot.Collections;

public partial class Game : Node2D
{
	private ActionPane ActionPane => GetNode<ActionPane>("ActionPane");
	public Map Map => GetNode<Map>("CurrentMap");
	public Player Player => GetNode<Player>("Player");

	public Control WinOverlay => GetNode<Control>("WinOverlay");
	public Control LoseOverlay => GetNode<Control>("LoseOverlay");

	private ActionPlayer ActionPlayer => GetNode<ActionPlayer>("ActionPlayer");

	public override void _Ready()
	{
		base._Ready();
		Player.Position = Map.SpawnPosition;
	}

	public void OnPlayPressed()
	{
		GD.Print("Play");
		ActionPlayer.Play(this.ActionPane.Actions);
	}

	public void OnResetPressed()
	{
		GD.Print("Reset");
		Player.Position = Map.SpawnPosition;
		ActionPlayer.Reset();
	}

	public void OnPlayerWon()
	{
		this.WinOverlay.Show();
	}

	public void OnPlayerLost()
	{
		this.LoseOverlay.Show();
	}
}