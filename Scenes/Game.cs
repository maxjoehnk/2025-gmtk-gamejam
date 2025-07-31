using gmtkgamejam.Scenes;
using Godot;
using Godot.Collections;

public partial class Game : Node2D
{
	private ActionPane ActionPane => this.GetNode<ActionPane>("ActionPane");
	public Player Player => this.GetNode<Player>("Player");

	public Control WinOverlay => this.GetNode<Control>("WinOverlay");
	public Control LoseOverlay => this.GetNode<Control>("LoseOverlay");

	private ActionPlayer ActionPlayer => this.GetNode<ActionPlayer>("ActionPlayer");
	
	[Export]
	public Vector2 SpawnPosition { get; set; }

	public void OnPlayPressed()
	{
		GD.Print("Play");
		this.ActionPlayer.Play(this.ActionPane.Actions);
	}

	public void OnResetPressed()
	{
		GD.Print("Reset");
		this.Respawn();
		this.ActionPlayer.Reset();
		this.WinOverlay.Hide();
		this.LoseOverlay.Hide();
	}

	public void Respawn()
	{
		this.Player.Position = this.SpawnPosition;
		this.Player.RotationDegrees = 0;
	}

	public void OnPlayerWon()
	{
		this.WinOverlay.Show();
		this.ActionPlayer.Stop();
	}

	public void OnPlayerLost()
	{
		this.LoseOverlay.Show();
		this.ActionPlayer.Stop();
	}
}