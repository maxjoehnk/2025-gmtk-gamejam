using System.Linq;
using gmtkgamejam.Scenes;
using Godot;

namespace gmtkgamejam.Core;

public partial class LevelLoader : Node
{
	public static LevelLoader Instance;
	public Node CurrentScene { get; set; }

	public override void _Ready()
	{
		Instance = this;
		Viewport root = this.GetTree().Root;
		this.CurrentScene = root.GetChild(-1);
	}

	public AvailableLevel[] GetAvailableLevels()
	{
		AvailableLevel[] levels = ResourceLoader.ListDirectory("res://Scenes/Levels")
			.Where(name => name.EndsWith(".tscn")).Select(file => new AvailableLevel(file)).ToArray();

		return levels;
	}

	public void LoadLevel(AvailableLevel level)
	{
		this.CallDeferred(nameof(this.LoadScene), $"res://Scenes/Levels/{level.Path}");
	}

	public void OpenLevelSelector()
	{
		this.CallDeferred(nameof(this.LoadScene), "res://Scenes/UI/LevelSelect/LevelSelector.tscn");
	}

	public void LoadScene(string path)
	{
		this.CurrentScene.QueueFree();
		PackedScene scene = GD.Load<PackedScene>(path);
		this.CurrentScene = scene.Instantiate();

		this.GetTree().Root.AddChild(this.CurrentScene);
		this.GetTree().CurrentScene = this.CurrentScene;
	}
}