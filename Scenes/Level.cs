using Godot;

namespace gmtkgamejam.Scenes;

public partial class Level : GodotObject
{
	public string Path { get; }

	public string Name => this.Path.Replace(".tscn", "");
	
	public Level(string path)
	{
		this.Path = path;
	}

	public PackedScene Load()
	{
		return GD.Load<PackedScene>($"res://Scenes/Levels/{this.Path}");
	}
}