using Godot;
using gmtkgamejam.Core;
using Vector2 = Godot.Vector2;

public partial class Player : StaticBody2D
{
	public void Move(MoveDirection direction)
	{
		this.RotationDegrees = GetRotation(direction);
		Vector2 moveVector = direction.ToMovementVector();

		KinematicCollision2D? collision2D = MoveAndCollide(moveVector, testOnly: true);
		if (collision2D != null)
		{
			return;
		}

		MoveAndCollide(moveVector);
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