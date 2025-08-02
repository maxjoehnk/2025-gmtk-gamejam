using System.Collections.Generic;
using System.Linq;
using gmtkgamejam.Scenes;
using gmtkgamejam.Scripts.Core;
using Godot;
using Microsoft.Win32;

namespace gmtkgamejam.Core;

public partial class LevelManager : Node
{
	public static LevelManager Instance;
	public Node CurrentScene { get; set; }

	private int? lastFinishedLevelIndex;
	private int? currentLevelIndex;

	private List<AvailableLevel> levels = [];

	public override void _Ready()
	{
		Instance = this;
		Viewport root = this.GetTree().Root;
		this.CurrentScene = root.GetChild(-1);
		this.levels = GetAvailableLevels();
		this.ReloadUserData();
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
		return this.levels.Count > this.currentLevelIndex + 1;
	}

	public bool HasCompletedALevel()
	{
		return this.lastFinishedLevelIndex != null;
	}

	/// <summary>
	/// Method to load the current level from the main menu.
	/// </summary>
	public void LoadCurrentLevel()
	{
		AvailableLevel currentLevel =
			this.currentLevelIndex == null ? this.levels.First() : this.levels[this.currentLevelIndex.Value];
		this.LoadLevel(currentLevel);
	}

	/// <summary>
	/// Method to load the next level from the level completion screen.
	/// </summary>
	public void LoadNextLevel()
	{
		if (this.lastFinishedLevelIndex == null)
		{
			GD.PrintErr("No last finished level index set, cannot load next level.");
			return;
		}
		AvailableLevel nextLevel = this.levels[this.lastFinishedLevelIndex.Value + 1];
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

	public void LevelFinished(int currentTick, bool hasGoldMedal, bool hasSilverMedal, bool hasBronzeMedal)
	{
		if (this.currentLevelIndex == null)
		{
			GD.PrintErr("No current level index set, cannot finish level.");
			return;
		}
		UserDataManager.StoreFinishedLevel(this.currentLevelIndex.Value);
		this.lastFinishedLevelIndex = this.currentLevelIndex;
	}

	public void ReloadUserData()
	{
		this.lastFinishedLevelIndex = UserDataManager.LoadFinishedLevel();
		if (this.lastFinishedLevelIndex != null)
		{
			this.currentLevelIndex = this.lastFinishedLevelIndex + 1;
		}
	}
}