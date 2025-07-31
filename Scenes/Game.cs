using gmtkgamejam.Core;
using gmtkgamejam.Scenes;
using Godot;
using Godot.Collections;

public partial class Game : Node2D
{
	private ActionPane ActionPane => this.GetNode<ActionPane>("ActionPane");
	public Player Player => this.GetNode<Player>("Player");

	public Control WinOverlay => this.GetNode<Control>("WinOverlay");
	public Control LoseOverlay => this.GetNode<Control>("LoseOverlay");

	public ActionPlayer ActionPlayer => this.GetNode<ActionPlayer>("ActionPlayer");
	
	private Node2D PreviewIndicator => this.GetNode<Node2D>("PreviewIndicator");
	
	[Export]
	public Vector2 SpawnPosition { get; set; }

	public override void _Ready()
	{
		this.ActionPane.ActionsChanged += OnActionsUpdated;
		this.PreviewIndicator.Hide();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.IsActionJustPressed("OpenMenu"))
		{
			LevelLoader.Instance.OpenLevelSelector();
		}

        if (Input.IsActionJustPressed("Play"))
        {
            OnPlayPressed();
        }

        if (Input.IsActionJustPressed("Reset"))
        {
            OnResetPressed();
        }
    }

	public void OnPlayPressed()
	{
		GD.Print("Play");
		this.PreviewIndicator.Hide();
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

	public void OnActionsUpdated()
	{
		Vector2 position = this.SpawnPosition;
		foreach (Action action in this.ActionPane.Actions)
		{
			position = action.Preview(position);
		}
		
		this.PreviewIndicator.GlobalPosition = position;
		this.PreviewIndicator.Show();
	}

	public void OnActionsFinished()
	{
		this.PreviewIndicator.Show();
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