using Godot;

namespace gmtkgamejam.Core;

public partial class MoveAction : Action
{
    public MoveDirection Direction { get; set; }
    public override string Title => "Move " + Direction.ToString();
    
    public override void Act(Player player)
    {
        GD.Print("Move");
    }
}