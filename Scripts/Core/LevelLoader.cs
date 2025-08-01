using System.Collections.Generic;
using System.Linq;
using gmtkgamejam.Scenes;
using Godot;

namespace gmtkgamejam.Core;

public partial class LevelLoader : Node
{
	public static LevelLoader Instance;
	public Node CurrentScene { get; set; }
	
	private int currentLevelIndex = -1;
	
	private List<AvailableLevel> levels = [];

	public override void _Ready()
	{
		Instance = this;
		Viewport root = this.GetTree().Root;
		this.CurrentScene = root.GetChild(-1);
		this.levels = GetAvailableLevels();
	}

	public static List<AvailableLevel> GetAvailableLevels()
	{
		List<AvailableLevel> levels = ResourceLoader.ListDirectory("res://Scenes/Levels")
			.Where(name => name.EndsWith(".tscn")).Select(file => new AvailableLevel(file)).ToList();

		return levels;
	}

	public void LoadLevel(AvailableLevel level)
	{
		this.currentLevelIndex = this.levels.IndexOf(level);
		this.CallDeferred(nameof(this.LoadScene), $"res://Scenes/Levels/{level.Path}");
	}

	public bool HasNextLevel()
	{
		return levels.Count > this.currentLevelIndex + 1;
	}
	
	public void LoadNextLevel()
	{
		AvailableLevel nextLevel = levels[++this.currentLevelIndex];
		this.LoadLevel(nextLevel);
	}

	public void OpenLevelSelector()
	{
		this.CallDeferred(nameof(this.LoadScene), "res://Scenes/UI/LevelSelect/LevelSelector.tscn");
	}

	public void OpenMainMenu()
	{
		this.CallDeferred(nameof(this.LoadScene), "res://Scenes/UI/MainMenu.tscn");
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