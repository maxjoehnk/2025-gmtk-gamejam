using Godot;
using gmtkgamejam.Core;
using gmtkgamejam.Scenes;

public partial class LevelSelector : Control
{
	private PackedScene LevelButton => GD.Load<PackedScene>("res://Scenes/UI/LevelSelect/LevelButton.tscn");
	
	public override void _Ready()
	{
		foreach (AvailableLevel level in LevelManager.Instance.Levels)
		{
			LevelButton levelButton = this.CreateLevelButton(level);

			this.AddChild(levelButton);
		}
	}

	private LevelButton CreateLevelButton(AvailableLevel level)
	{
		LevelButton levelButton = this.LevelButton.Instantiate<LevelButton>();
		levelButton.LevelName = level.Name;
		levelButton.Disabled = !level.IsUnlocked;
		levelButton.IsTest = level.IsTestLevel;
		if (level.IsUnlocked)
		{
			levelButton.Pressed += () => { LevelManager.Instance.LoadLevel(level); };
		}
		
		return levelButton;
	}

	public void OnBackPressed()
	{
		LevelManager.Instance.OpenMainMenu();
	}
}

