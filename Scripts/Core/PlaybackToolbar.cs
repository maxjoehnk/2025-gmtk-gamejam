using Godot;
using gmtkgamejam.Scripts.Core;

public partial class PlaybackToolbar : HBoxContainer, IClocked
{
	private Label TickLabel => GetNode<Label>("Label");

	public void OnTick(int tick)
	{
		this.TickLabel.Text = "Tick: " + tick;
	}
}
