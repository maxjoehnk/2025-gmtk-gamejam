using gmtkgamejam.Core;
using Godot;
using gmtkgamejam.Scenes;
using gmtkgamejam.Scripts.Core;

public partial class TeleporterEntry : Node2D, ISwitchable, IInteractable, IResettable
{
	[Export] public bool IsActive { get; set; } = true;

	[Export] public NodePath Target { get; set; }
	
	public TeleporterExit? Exit => this.GetNode<TeleporterExit>(this.Target);
	
	private AnimatedSprite2D Sprite => this.GetNode<AnimatedSprite2D>("Sprite");
	
	private bool CurrentAnimationState => this.Sprite.Animation != "Inactive";

	private Label? DebugLabel { get; set; }

	public override void _Ready()
	{
		this.State = this.IsActive;
		if (OS.IsDebugBuild() && Constants.ShowDebugInfo)
		{
			this.DebugLabel = new Label();
			this.DebugLabel.SetAnchorsPreset(Control.LayoutPreset.TopLeft);
			this.Sprite.AddChild(this.DebugLabel);
		}
	}

	public override void _Process(double delta)
	{
		this.Sprite.SpeedScale = (float)ActionPlayer.Instance.PlaybackSpeed;
		if (this.DebugLabel != null)
		{
			this.DebugLabel.Text = $"State: {this.State}\nActive: {this.IsActive}\nCurrent Animation State: {this.CurrentAnimationState}\nAnimation: {this.Sprite.Animation}";
		}
		if (this.State != this.CurrentAnimationState)
		{
			this.Sprite.Play(this.State ? "Active" : "Inactive");
		}
	}

	public void Interact(Player player)
	{
		if (!this.State)
		{
			return;
		}
		if (this.Exit == null)
		{
			GD.PrintErr($"TeleporterExit not found at path: {this.Target}");
			return;
		}

		player.GlobalPosition = this.Exit.GlobalPosition;
	}

	public void Toggle()
	{
		this.State = !this.State;
	}

	public bool State { get; set; }

	public void Reset()
	{
		this.State = this.IsActive;
		this.Sprite.Play(this.State ? "Active" : "Inactive");
	}
}