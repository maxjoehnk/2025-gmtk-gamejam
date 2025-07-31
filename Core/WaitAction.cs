using Godot;

namespace gmtkgamejam.Core;

public partial class WaitAction : Action
{
	public override string Title => "Wait";
	public override void Act(Player player)
	{
		GD.Print("Wait");
	}
}
