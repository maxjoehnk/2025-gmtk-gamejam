using Godot;
using gmtkgamejam.Scenes;

public partial class GuardPath : PathFollow2D
{
	private ActionPlayer Playback => GetNode<ActionPlayer>("/root/Game/ActionPlayer");
	private Guard Guard => GetChild<Guard>(0);

	public override void _Ready()
	{
		Playback.Ticked += OnTick;
	}

	private void OnTick(int tick)
	{
		this.Progress = this.Guard.Speed * tick;
	}
}
