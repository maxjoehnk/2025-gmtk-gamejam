using Godot;

namespace gmtkgamejam.Scenes;

public partial class AvailableLevel : GodotObject
{
	public string Path { get; }

	public string Name => this.Path.Replace(".tscn", "");
	
	public AvailableLevel(string path)
	{
		this.Path = path;
	}
}