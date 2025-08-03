using Godot;
using System;

public partial class TransitionScreen : TextureRect
{
	public static TransitionScreen Instance { get; set; } = null!;
	
	private AnimationPlayer AnimationPlayer => this.GetNode<AnimationPlayer>("AnimationPlayer");

	private Action? lastAction;

	public override void _Ready()
	{
		Instance = this;
		this.Visible = false;
		this.Position = new Vector2(0, -1080);
	}

	public void EnterTransition(Action action)
	{
		this.Visible = true;
		this.lastAction = action;
		this.AnimationPlayer.Play("Enter");
	}

	public void ExitTransition()
	{
		this.AnimationPlayer.Play("Exit");
	}

	public void OnAnimationFinished(StringName animationName)
	{
		if (animationName == "Exit")
		{
			this.Visible = false;
		}

		if (this.lastAction != null)
		{
			this.lastAction.Invoke();
			this.lastAction = null;
		}
	}
}
