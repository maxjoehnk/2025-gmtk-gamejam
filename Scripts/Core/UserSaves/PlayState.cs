using Godot;

namespace gmtkgamejam.Scripts.Core;

public partial class PlayState : Resource
{
	[Export] public int LastPlayedLevelIndex { get; set; } = -1;
}