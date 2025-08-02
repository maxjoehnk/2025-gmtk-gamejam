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
	
	private GpuParticles2D Particles => this.GetNode<GpuParticles2D>("GPUParticles2D");
	
	private bool CurrentAnimationState => this.Sprite.Animation != "Inactive";

	private Label? DebugLabel { get; set; }

	private Tween? tween;

	public override void _Ready()
	{
		this.Reset();
		this.Sprite.SpeedScale = 1;
		if (OS.IsDebugBuild() && Constants.ShowDebugInfo)
		{
			this.DebugLabel = new Label();
			this.DebugLabel.SetAnchorsPreset(Control.LayoutPreset.TopLeft);
			this.Sprite.AddChild(this.DebugLabel);
		}
	}

	public override void _Process(double delta)
	{
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

		this.AnimateTeleporterEffects();
		player.GlobalPosition = this.Exit.GlobalPosition;
	}

	private void AnimateTeleporterEffects()
	{
		float amountRatio = this.Particles.AmountRatio;
		this.Particles.AmountRatio = 1;
		float speed = this.Sprite.SpeedScale;
		Tween fadeOutTween = this.CreateTween();
		fadeOutTween.SetParallel();
		fadeOutTween.TweenProperty(this.Particles, "amount_ratio", amountRatio, 2f);
		fadeOutTween.TweenProperty(this.Sprite, "speed_scale", speed, 1f);
		this.tween = this.CreateTween();
		this.tween.TweenProperty(this.Sprite, "speed_scale", 4, 0.1f);
		this.tween.TweenSubtween(fadeOutTween);
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