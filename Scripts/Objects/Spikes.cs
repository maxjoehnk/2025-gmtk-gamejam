using Godot;
using gmtkgamejam.Scenes;
using gmtkgamejam.Scenes.Enemies;
using gmtkgamejam.Scripts.Core;

public partial class Spikes : Enemy, ISwitchable, IResettable
{
	private Area2D Collider => this.GetNode<Area2D>("Spikes");
	private AnimatedSprite2D Animation => this.GetNode<AnimatedSprite2D>("Spikes/Spikes");

	[Export] public bool IsActive { get; set; } = true;
	
	private bool CurrentAnimationState => this.Animation.Animation == "Up";

	public bool State { get; set; } = true;

	public override void _Process(double delta)
	{
		this.Collider.ProcessMode = this.State ? ProcessModeEnum.Inherit : ProcessModeEnum.Disabled;
		this.Animation.SpeedScale = (float)ActionPlayer.Instance.PlaybackSpeed;
		if (this.State != this.CurrentAnimationState)
		{
			this.Animation.Play(this.State ? "Up" : "Down");
		}
	}
	
	public void Toggle()
	{
		this.State = !this.State;
	}

	public void Reset()
	{
		this.State = this.IsActive;
		this.Animation.Play(this.State ? "Up" : "Down");
	}
}