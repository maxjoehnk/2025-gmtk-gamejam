using Godot;
using System;
using gmtkgamejam.Scripts.Core;

public partial class Spikes : Node2D, ISwitchable, IResettable
{
	private StaticBody2D Collider => this.GetNode<StaticBody2D>("Spikes");
	private Sprite2D Collapsed => this.GetNode<Sprite2D>("Spikes/Spikes");
	private Sprite2D Expanded => this.GetNode<Sprite2D>("Spikes/SpikesExpanded");

	[Export] public bool IsActive { get; set; } = true;
	
	private bool state = true;

	public override void _Process(double delta)
	{
		this.Collapsed.Visible = !this.state;
		this.Expanded.Visible = this.state;
		this.Collider.ProcessMode = this.state ? ProcessModeEnum.Inherit : ProcessModeEnum.Disabled;
	}

	public override void _Ready()
	{
		this.state = IsActive;
	}

	public void Toggle()
	{
		this.state = !this.state;
	}

	public void Reset()
	{
		this.state = IsActive;
	}
}
