using Godot;
using static gmtkgamejam.Core.Constants;

namespace gmtkgamejam.Core;

public static class MoveDirectionExtensions
{
	public static Vector2 ToMovementVector(this MoveDirection direction)
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