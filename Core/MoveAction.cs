namespace gmtkgamejam.Core;

public partial class MoveAction : Action
{
    public MoveDirection Direction { get; set; }
    public override string Title => "Move " + Direction.ToString();
}