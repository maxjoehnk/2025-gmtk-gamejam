using gmtkgamejam.Core;
using gmtkgamejam.Scenes;
using gmtkgamejam.Scripts.Core;
using Godot;

public partial class Game : Node2D
{
	private ActionPane ActionPane => this.GetNode<ActionPane>("View/Canvas/ActionPane");
	public Player Player => this.GetNode<Player>("Player");

	public WinOverlay WinOverlay => this.GetNode<WinOverlay>("View/WinOverlay");
	public CaughtOverlay CaughtOverlay => this.GetNode<CaughtOverlay>("View/CaughtOverlay");
	public PauseMenuOverlay PauseMenuOverlay => this.GetNode<PauseMenuOverlay>("View/PauseMenuOverlay");

	private HSlider SpeedSlider => this.GetNode<HSlider>("View/VBoxContainer/SpeedSliderToolbar/HSlider");

	private PreviewIndicator PreviewIndicator => this.GetNode<PreviewIndicator>("PreviewIndicator");

	private double prePreviewSpeed = 1;

	[Export] public Vector2 SpawnPosition { get; set; }

	public GameState CurrentGameState = GameState.Stop;

	public override void _Ready()
	{
		this.SpeedSlider.ValueChanged += value => { ActionPlayer.Instance.PlaybackSpeed = value; };
		this.ActionPane.ActionsChanged += this.OnActionsChanged;
		ActionPlayer.Instance.Finished += this.OnActionsFinished;
		ActionPlayer.Instance.PlaybackSpeed = this.SpeedSlider.Value;
		this.PreviewIndicator.Hide();
		this.WinOverlay.Hide();
		this.CaughtOverlay.Hide();
		this.CurrentGameState = GameState.Prepare;
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

	private void OnActionsChanged()
	{
		this.PreviewIndicator.Show();
	}

	public void OnPausePressed()
	{
		this.PauseMenuOverlay.Open();
		this.CurrentGameState = GameState.Stop;
	}

	public void OnPlayPressed()
	{
		GD.Print("Play");
		this.OnResetPressed();
		this.PreviewIndicator.Hide();
		ActionPlayer.Instance.Play(this.ActionPane.Actions);
		this.CurrentGameState = GameState.Playing;
	}

	public void OnResetPressed()
	{
		GD.Print("Reset");
		this.Respawn();
		ActionPlayer.Instance.Reset();
		this.CaughtOverlay.Hide();
		this.WinOverlay.Hide();
		ResetGameElements();
		this.CurrentGameState = GameState.Prepare;
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
				position = action.Preview(position, this.PreviewIndicator);
			}
		}

		this.PreviewIndicator.GlobalPosition = position;
	}

	public void OnActionsFinished()
	{
		this.PreviewIndicator.Show();
		this.CurrentGameState = GameState.Prepare;
	}

	public void Respawn()
	{
		this.Player.Position = this.SpawnPosition;
		this.Player.RotationDegrees = 0;
	}

	public void OnPlayerWon(int goldMedalTicks, int silverMedalTicks, int bronzeMedalTicks)
	{
		// We've lost in the same move were we've reached the goal so we actually lost
		if (this.CaughtOverlay.IsVisible())
		{
			return;
		}

		int currentTick = ActionPlayer.Instance.CurrentTick;
		bool hasGoldMedal = goldMedalTicks >= currentTick;
		bool hasSilverMedal = silverMedalTicks >= currentTick;
		bool hasBronzeMedal = bronzeMedalTicks >= currentTick;
		this.WinOverlay.Open(currentTick, hasGoldMedal, hasSilverMedal, hasBronzeMedal);
		this.WinOverlay.Show();
		ActionPlayer.Instance.Stop();
		this.CurrentGameState = GameState.Stop;
		LevelManager.Instance.LevelFinished(currentTick, hasGoldMedal, hasSilverMedal, hasBronzeMedal);
	}

	public void OnPlayerLost()
	{
		this.CaughtOverlay.Open();
		ActionPlayer.Instance.Stop();
		this.CurrentGameState = GameState.Stop;
	}
}