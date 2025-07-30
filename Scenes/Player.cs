using Godot;
using gmtkgamejam.Core;
using Vector2 = Godot.Vector2;

public partial class Player : StaticBody2D
{
	private const int GridSize = 128;

	public void Move(MoveDirection direction)
	{
		Vector2 moveVector = GetMovementVector(direction);

		KinematicCollision2D? collision2D = MoveAndCollide(moveVector, testOnly: true);
		if (collision2D != null)
		{
			return;
		}

		MoveAndCollide(moveVector);
	}

	private static Vector2 GetMovementVector(MoveDirection direction)
	{
		if (direction == MoveDirection.Up)
		{
			return new Vector2(0, -GridSize);
		}

		if (direction == MoveDirection.Left)
		{
			return new Vector2(-GridSize, 0);
		}

		if (direction == MoveDirection.Right)
		{
			return new Vector2(GridSize, 0);
		}

		if (direction == MoveDirection.Down)
		{
			return new Vector2(0, GridSize);
		}

		return Vector2.Zero;
	}
}