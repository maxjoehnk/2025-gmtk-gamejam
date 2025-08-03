using Godot;

namespace gmtkgamejam.Core;

public abstract partial class Action : Control
{
	[Signal]
	public delegate void ActionAppliedEventHandler();

	public int Ticks { get; set; } = 1;

	public abstract string Title { get; }

	public abstract void Act(Player player);

	public virtual Vector2 Preview(Vector2 position, PreviewIndicator indicatorNode)
	{
		return position;
	}
}