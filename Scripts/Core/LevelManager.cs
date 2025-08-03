using System.Collections.Generic;
using System.Linq;
using gmtkgamejam.Scenes;
using gmtkgamejam.Scripts.Core;
using Godot;

namespace gmtkgamejam.Core;

public partial class LevelManager : Node
{
	public static LevelManager Instance = null!;
	private Node CurrentScene { get; set; } = null!;

	private int? lastFinishedLevelIndex;
	private int? currentLevelIndex;
	
	public AvailableLevel? CurrentLevel => 
		this.currentLevelIndex == null ? null : this.Levels[this.currentLevelIndex.Value];

	public List<AvailableLevel> Levels { get; private set; } = [];

	public override void _Ready()
	{
		Instance = this;
		GD.Print(OS.GetDataDir());
		GD.Print(OS.GetUserDataDir());
		Viewport root = this.GetTree().Root;
		this.CurrentScene = root.GetChild(-1);
		this.Levels = GetAvailableLevels();
		this.ReloadUserData();
		this.UnlockLevels();
	}

	private static List<AvailableLevel> GetAvailableLevels()
	{
		List<AvailableLevel> levels = ResourceLoader.ListDirectory("res://Scenes/Levels")
			.Where(name => name.EndsWith(".tscn"))
			.Select((file, index) => new AvailableLevel(file, index))
			.Where(level => !level.IsTestLevel || OS.IsDebugBuild())
			.OrderBy(level => level.LevelIndex)
			.ToList();

		return levels;
	}

	public void LoadLevel(AvailableLevel level)
	{
		this.currentLevelIndex = this.Levels.IndexOf(level);
		this.TransitionToScene($"res://Scenes/Levels/{level.Path}");
	}

	public bool HasNextLevel()
	{
		return this.Levels.Count > this.currentLevelIndex + 1;
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
			this.currentLevelIndex == null ? this.Levels.First() : this.Levels[this.currentLevelIndex.Value];
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
		AvailableLevel nextLevel = this.Levels[this.lastFinishedLevelIndex.Value + 1];
		this.LoadLevel(nextLevel);
	}

	public void OpenLevelSelector()
	{
		this.TransitionToScene("res://Scenes/UI/LevelSelect/LevelSelector.tscn");
	}

	public void OpenMainMenu()
	{
		this.TransitionToScene("res://Scenes/UI/MainMenu.tscn");
	}

	public void OpenSettings()
	{
		this.TransitionToScene("res://Scenes/UI/Settings.tscn");
	}

	public void TransitionToScene(string scenePath)
	{
		TransitionScreen.Instance.EnterTransition(() =>
		{
			this.CallDeferred(nameof(this.LoadScene), scenePath);
		});
	}

	public void LoadScene(string path)
	{
		this.CurrentScene.QueueFree();
		PackedScene scene = GD.Load<PackedScene>(path);
		this.CurrentScene = scene.Instantiate();

		this.GetTree().Root.AddChild(this.CurrentScene);
		this.GetTree().CurrentScene = this.CurrentScene;
		TransitionScreen.Instance.ExitTransition();
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
		this.UnlockLevels();
	}

	public void ReloadUserData()
	{
		this.lastFinishedLevelIndex = UserDataManager.LoadFinishedLevel();
		if (this.lastFinishedLevelIndex != null)
		{
			this.currentLevelIndex = this.lastFinishedLevelIndex + 1;
		}
		else
		{
			this.currentLevelIndex = null;
		}
		this.UnlockLevels();
	}

	private void UnlockLevels()
	{
		if (OS.IsDebugBuild())
		{
			foreach (AvailableLevel level in this.Levels)
			{
				level.IsUnlocked = true;
			}
			return;
		}
		
		for (int i = 0; i < this.Levels.Count; i++)
		{
			if (i == 0)
			{
				this.Levels[0].IsUnlocked = true;
				continue;
			}

			this.Levels[i].IsUnlocked = this.lastFinishedLevelIndex != null && i <= this.lastFinishedLevelIndex + 1;
		}
	}
}