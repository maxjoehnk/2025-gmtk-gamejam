using Godot;

namespace gmtkgamejam.Core;

public partial class InteractAction : Action
{
    public override string Title => "Interact";
    public override void Act(Player player)
    {
        GD.Print("Interact");
    }
}