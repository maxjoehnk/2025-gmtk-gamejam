using Godot;

namespace gmtkgamejam.Scripts.Core;

public partial class Settings : Resource
{
	[Export]
	public double MainVolume { get; set; }

	[Export]
	public double MusicVolume { get; set; }

	[Export]
	public double EffectsVolume { get; set; }
}