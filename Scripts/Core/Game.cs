using System.Text.RegularExpressions;
using gmtkgamejam.Core;
using gmtkgamejam.Scenes;
using gmtkgamejam.Scripts.Core;
using Godot;
using Godot.Collections;
using static Godot.Control;

public partial class Game : Node2D
{
  private ActionPane ActionPane => this.GetNode<ActionPane>("ActionPane");
  public Player Player => this.GetNode<Player>("Player");

  public WinOverlay WinOverlay => this.GetNode<WinOverlay>("WinOverlay");
  public Control LoseOverlay => this.GetNode<Control>("LoseOverlay");

  public ActionPlayer ActionPlayer => this.GetNode<ActionPlayer>("ActionPlayer");

  private HSlider SpeedSlider => this.GetNode<HSlider>("VBoxContainer/SpeedSliderToolbar/HSlider");

  private CharacterBody2D PreviewIndicator => this.GetNode<CharacterBody2D>("PreviewIndicator");

  private double prePreviewSpeed = 1;

  [Export] public Vector2 SpawnPosition { get; set; }

  public override void _Ready()
  {
    this.ActionPane.ActionsChanged += OnActionsUpdated;
    this.SpeedSlider.ValueChanged += value =>
    {
      this.ActionPlayer.PlaybackSpeed = value;
    };
    this.PreviewIndicator.Hide();
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if(Input.IsActionJustPressed("OpenMenu"))
    {
      LevelLoader.Instance.OpenLevelSelector();
    }

    if(Input.IsActionJustPressed("Play"))
    {
      OnPlayPressed();
    }

    if(Input.IsActionJustPressed("Reset"))
    {
      OnResetPressed();
    }
  }

  public void OnPlayPressed()
  {
    GD.Print("Play");
    this.OnResetPressed();
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
    ResetGameElements();
	}

  public void OnLoopPressed()
  {
    GD.Print("Preview started");
    this.prePreviewSpeed = ActionPlayer.PlaybackSpeed;
    ActionPlayer.PlaybackSpeed = Constants.PreviewPlaybackSpeed;
    ActionPlayer.Preview = true;
    this.OnPlayPressed();
  }

  public void OnLoopReleased()
  {
    GD.Print("Preview stopped");
    this.OnResetPressed();
    ActionPlayer.Preview = false;
    ActionPlayer.PlaybackSpeed = this.prePreviewSpeed;
  }

	private void ResetGameElements()
	{
		foreach (Node node in GetTree().GetNodesInGroup(Groups.Resettable))
		{
			if (node is IResettable resettable)
			{
				resettable.Reset();
			}
		}
  }

  public void OnActionsUpdated()
  {
    Vector2 position = this.SpawnPosition;
    this.PreviewIndicator.GlobalPosition = position;
    foreach(Action action in this.ActionPane.Actions)
    {
      Vector2 nextPosition = action.Preview(position);
      Vector2 positionDiff = nextPosition - position;

      KinematicCollision2D ? collision2D = this.PreviewIndicator.MoveAndCollide(positionDiff, testOnly: true);
      if(collision2D == null)
      {
        this.PreviewIndicator.MoveAndCollide(positionDiff);
        position = this.PreviewIndicator.GlobalPosition;
      }
    }

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

  public void OnPlayerWon(string name, int goldMedalTicks, int silverMedalTicks, int bronzeMedalTicks)
  {
    bool hasGoldMedal = goldMedalTicks >= this.ActionPlayer.CurrentTick;
    bool hasSilverMedal = silverMedalTicks >= this.ActionPlayer.CurrentTick;
    bool hasBronzeMedal = bronzeMedalTicks >= this.ActionPlayer.CurrentTick;
    this.WinOverlay.Open(name, this.ActionPlayer.CurrentTick, hasGoldMedal, hasSilverMedal, hasBronzeMedal);
    this.WinOverlay.Show();
    this.ActionPlayer.Stop();
  }

  public void OnPlayerLost()
  {
    this.LoseOverlay.Show();
    this.ActionPlayer.Stop();
  }
}