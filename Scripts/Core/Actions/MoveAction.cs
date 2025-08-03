using Godot;

namespace gmtkgamejam.Core;

public partial class MoveAction : Action
{
	public MoveDirection Direction { get; set; }
	public override string Title => "Move " + this.Direction;

	public override void Act(Player player)
	{
		player.Move(this.Direction);
		this.EmitSignalActionApplied();
	}

	public override Vector2 Preview(Vector2 position, PreviewIndicator indicator)
	{
		Vector2 movement = this.Direction.ToMovementVector();

		KinematicCollision2D? collision2D = indicator.MoveAndCollide(movement, testOnly: true);
		if (collision2D != null)
		{
			return position;
		}
		return position + movement;
	}
}