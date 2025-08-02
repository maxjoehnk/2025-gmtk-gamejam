using Godot;

namespace gmtkgamejam.Core;

public partial class MoveAction : Action
{
    public MoveDirection Direction { get; set; }
    public override string Title => "Move " + Direction;
    
    public override void Act(Player player)
    {
        GD.Print("Move");
        player.Move(Direction);
    }

    public override Vector2 Preview(Vector2 position)
    {
        return position + this.Direction.ToMovementVector();
    }
}