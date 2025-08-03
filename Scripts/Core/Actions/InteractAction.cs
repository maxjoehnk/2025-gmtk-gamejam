using gmtkgamejam.Scripts.Core;
using Godot;

namespace gmtkgamejam.Core;

public partial class InteractAction : Action
{
	public override string Title => "Interact";

	public override void Act(Player player)
	{
		player.GetInteractableElement()?.Interact(player);
		this.EmitSignalActionApplied();
	}

	public override Vector2 Preview(Vector2 position, PreviewIndicator indicator)
	{
		IInteractable? interactable = indicator.GetInteractableElement();
		if (interactable is not TeleporterEntry entry)
		{
			return position;
		}

		return entry.Exit?.GlobalPosition ?? position;
	}
}