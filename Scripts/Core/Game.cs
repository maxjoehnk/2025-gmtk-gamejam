using gmtkgamejam.Core;
using gmtkgamejam.Scenes;
using gmtkgamejam.Scripts.Core;
using Godot;

public partial class Game : Node2D
{
	private ActionPane ActionPane => this.GetNode<ActionPane>("ActionPane");
	public Player Player => this.GetNode<Player>("Player");

	public WinOverlay WinOverlay => this.GetNode<WinOverlay>("WinOverlay");
	public CaughtOverlay CaughtOverlay => this.GetNode<CaughtOverlay>("CaughtOverlay");
    public PauseMenuOverlay PauseMenuOverlay => this.GetNode<PauseMenuOverlay>("PauseMenuOverlay");

    private HSlider SpeedSlider => this.GetNode<HSlider>("VBoxContainer/SpeedSliderToolbar/HSlider");

	private CharacterBody2D PreviewIndicator => this.GetNode<CharacterBody2D>("PreviewIndicator");

	private double prePreviewSpeed = 1;

	[Export] public Vector2 SpawnPosition { get; set; }

	public override void _Ready()
	{
		this.SpeedSlider.ValueChanged += value => { ActionPlayer.Instance.PlaybackSpeed = value; };
		this.ActionPane.ActionsChanged += () => { this.PreviewIndicator.Show(); };
		ActionPlayer.Instance.Finished += this.OnActionsFinished;
		ActionPlayer.Instance.PlaybackSpeed = this.SpeedSlider.Value;
		this.PreviewIndicator.Hide();
		this.WinOverlay.Hide();
		this.CaughtOverlay.Hide();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.IsActionJustPressed("OpenMenu"))
		{
			OnPausePressed();
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

	public override void _Process(double delta)
	{
		this.UpdateIndicatorPosition();
	}
    public void OnPausePressed()
    {
        this.PauseMenuOverlay.Open();
    }

    public void OnPlayPressed()
	{
		GD.Print("Play");
		this.OnResetPressed();
		this.PreviewIndicator.Hide();
		ActionPlayer.Instance.Play(this.ActionPane.Actions);
	}

	public void OnResetPressed()
	{
		GD.Print("Reset");
		this.Respawn();
		ActionPlayer.Instance.Reset();
		this.CaughtOverlay.Hide();
		this.WinOverlay.Hide();
		ResetGameElements();
	}

	public void OnLoopPressed()
	{
		GD.Print("Preview started");
		this.prePreviewSpeed = ActionPlayer.Instance.PlaybackSpeed;
		ActionPlayer.Instance.PlaybackSpeed = Constants.PreviewPlaybackSpeed;
		ActionPlayer.Instance.Preview = true;
		this.OnPlayPressed();
	}

	public void OnLoopReleased()
	{
		GD.Print("Preview stopped");
		this.OnResetPressed();
		ActionPlayer.Instance.Preview = false;
		ActionPlayer.Instance.PlaybackSpeed = this.prePreviewSpeed;
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

	private void UpdateIndicatorPosition()
	{
		Vector2 position = this.SpawnPosition;
		foreach (Action action in this.ActionPane.Actions)
		{
			for (int i = 0; i < action.Ticks; i++)
			{
				this.PreviewIndicator.GlobalPosition = position;
				Vector2 nextPosition = action.Preview(position);
				Vector2 positionDiff = nextPosition - position;

				KinematicCollision2D? collision2D = this.PreviewIndicator.MoveAndCollide(positionDiff, testOnly: true);
				if (collision2D == null)
				{
					position += positionDiff;
				}
			}
		}
		this.PreviewIndicator.GlobalPosition = position;
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
		// We've lost in the same move were we've reached the goal so we actually lost
		if (this.CaughtOverlay.IsVisible())
		{
			return;
		}

		bool hasGoldMedal = goldMedalTicks >= ActionPlayer.Instance.CurrentTick;
		bool hasSilverMedal = silverMedalTicks >= ActionPlayer.Instance.CurrentTick;
		bool hasBronzeMedal = bronzeMedalTicks >= ActionPlayer.Instance.CurrentTick;
		this.WinOverlay.Open(name, ActionPlayer.Instance.CurrentTick, hasGoldMedal, hasSilverMedal, hasBronzeMedal);
		this.WinOverlay.Show();
		ActionPlayer.Instance.Stop();
	}

	public void OnPlayerLost(string name)
	{
		this.CaughtOverlay.Open(name);
		ActionPlayer.Instance.Stop();
	}
}