using Godot;
using gmtkgamejam.Core;
using gmtkgamejam.Scenes;
using gmtkgamejam.Scripts.Core;
using Vector2 = Godot.Vector2;

public partial class Player : CharacterBody2D
{
	[Export] public float RotationSpeed { get; set; } = 360;

	[Signal]
	public delegate void PlayerLostEventHandler();

	private RayCast2D RayCast => this.GetNode<RayCast2D>("RayCast2D");

	private Vector2 nextDirection = new(0, 0);
	private float nextRotation;
	private float currentRotation;
	private double executedTime;
	private double lastExecutedRatio;

	public override void _Process(double delta)
	{
		if(this.GetParent<Game>().CurrentGameState != GameState.Playing)
		{
			this.executedTime = 0;
			this.lastExecutedRatio = 0;
			return;
		}
		if (this.nextDirection == new Vector2(0, 0))
		{
			return;
		}

		this.executedTime += delta;

		float maxRotationPerUpdate = this.RotationSpeed * (float)delta / (float)ActionPlayer.Instance.TickDuration;
		this.currentRotation +=
			Mathf.Clamp(this.nextRotation - this.currentRotation, -maxRotationPerUpdate, maxRotationPerUpdate);

		double baseRatio = Mathf.Min(1, this.executedTime / ActionPlayer.Instance.TickDuration);

		double lastRatioTranslation = Mathf.Sin(-Mathf.Pi * 0.5 + Mathf.Pi * this.lastExecutedRatio) * 0.5 + 0.5;
		double ratioTranslation = Mathf.Sin(-Mathf.Pi * 0.5 + Mathf.Pi * baseRatio) * 0.5 + 0.5;

		float ratioDiff = (float)(ratioTranslation - lastRatioTranslation);
		Vector2 movement = new(this.nextDirection.X * ratioDiff, this.nextDirection.Y * ratioDiff);
		this.MoveAndCollide(movement);
		this.RotationDegrees = this.currentRotation;
		this.lastExecutedRatio = baseRatio;
	}

	public void Move(MoveDirection direction)
	{
		this.nextDirection = direction.ToMovementVector();
		this.executedTime = 0;
		this.nextRotation = GetRotation(direction);

		if (Mathf.Abs(this.nextRotation - this.currentRotation) > 180)
		{
			this.currentRotation += this.nextRotation < this.currentRotation ? -360 : 360;
		}

		KinematicCollision2D? collision2D = this.MoveAndCollide(this.nextDirection, testOnly: true);
		if (collision2D != null)
		{
			// HACK: it would be better to check for an interface here, but the collider is not the actual script file because a guard uses a PathFollow2D
			if (collision2D.GetCollider() is Node2D { Owner: Guard })
			{
				this.EmitSignalPlayerLost();
			}

			this.nextDirection = new Vector2(0, 0);
			return;
		}

		this.lastExecutedRatio = 0;
	}

	public IInteractable? GetInteractableElement()
	{
		GodotObject collision = this.RayCast.GetCollider();
		if (collision is IInteractable interactable)
		{
			return interactable;
		}

		return null;
	}

	private static float GetRotation(MoveDirection direction)
	{
		if (direction == MoveDirection.Up)
		{
			return 0;
		}

		if (direction == MoveDirection.Left)
		{
			return 270;
		}

		if (direction == MoveDirection.Right)
		{
			return 90;
		}

		if (direction == MoveDirection.Down)
		{
			return 180;
		}

		return 0;
	}
}