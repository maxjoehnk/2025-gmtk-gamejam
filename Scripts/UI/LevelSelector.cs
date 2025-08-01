using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using gmtkgamejam.Core;
using gmtkgamejam.Scenes;
using Godot.Collections;

public partial class LevelSelector : Control
{
	private PackedScene LevelButton => GD.Load<PackedScene>("res://Scenes/UI/LevelSelect/LevelButton.tscn");
	
	public override void _Ready()
	{
		List<AvailableLevel> levels = LevelLoader.GetAvailableLevels();
		
		foreach (AvailableLevel level in levels)
		{
			LevelButton levelButton = this.CreateLevelButton(level);

			this.AddChild(levelButton);
		}
	}

	private LevelButton CreateLevelButton(AvailableLevel level)
	{
		LevelButton levelButton = this.LevelButton.Instantiate<LevelButton>();
		levelButton.LevelName = level.Name;
		levelButton.Pressed += () =>
		{
			LevelLoader.Instance.LoadLevel(level);
		};
		
		return levelButton;
	}
}

