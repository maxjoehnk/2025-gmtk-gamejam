using Godot;
using System;
using gmtkgamejam.Core;

public partial class LevelName : CenterContainer
{
	private Label LevelNameLabel => this.GetNode<Label>("Panel/LevelName");
	
	public override void _Ready()
	{
		this.Modulate = this.Modulate with { A = 0 };
		Tween tween = this.CreateTween();
		tween.TweenProperty(this, "modulate:a", 1, 0.2f);
		this.LevelNameLabel.Text = LevelManager.Instance.CurrentLevel?.Name;
	}

	public override void _Process(double delta)
	{
		// HACK: this is a workaround as I don't wanna figure out the layout system
		this.Size = new Vector2(1920, 500);
		this.Position = Vector2.Zero;
	}

	public void OnHide()
	{
			Tween tween = this.CreateTween();
			tween.TweenProperty(this, "modulate:a", 0, 0.25f);
			tween.TweenCallback(Callable.From(this.QueueFree));
	}
}
