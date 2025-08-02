using Godot;
using System;

public partial class TeleporterExit : Node2D
{
	private GpuParticles2D Particles => this.GetNode<GpuParticles2D>("GPUParticles2D");

	private Tween? tween;
	
	public void StartAnimation()
	{
		float amountRatio = this.Particles.AmountRatio;
		this.Particles.AmountRatio = 1;
		this.tween = this.CreateTween();
		this.tween.TweenProperty(this.Particles, "amount_ratio", amountRatio, 2f);
	}
}
