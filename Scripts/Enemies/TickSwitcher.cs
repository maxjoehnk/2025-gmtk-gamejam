using Godot;
using System.Collections.Generic;
using System.Linq;
using gmtkgamejam.Scripts.Core;

public partial class TickSwitcher : Node2D, IClocked
{
	[Export] public int Ticks { get; set; } = 1;

	private List<ISwitchable> Switches = [];
	
	public override void _Ready()
	{
		this.Switches = this.GetChildren().OfType<ISwitchable>().ToList();
		
		this.OnTick(0);
	}

	public void OnTick(int tick)
	{
		int switchIndex = (tick / this.Ticks) % this.Switches.Count;
		foreach (ISwitchable @switch in this.Switches)
		{
			@switch.State = @switch == this.Switches[switchIndex];
		}
	}
}
