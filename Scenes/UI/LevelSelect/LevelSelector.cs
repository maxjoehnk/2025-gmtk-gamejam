using Godot;
using System;
using System.Linq;
using gmtkgamejam.Core;
using gmtkgamejam.Scenes;
using Godot.Collections;

public partial class LevelSelector : Control
{
	private PackedScene LevelButton => GD.Load<PackedScene>("res://Scenes/UI/LevelSelect/LevelButton.tscn");
	
	public override void _Ready()
	{
		Level[] levels = ResourceLoader.ListDirectory("res://Scenes/Levels")
			.Where(name => name.EndsWith(".tscn")).Select(file => new Level(file)).ToArray();
		foreach (Level level in levels)
		{
			GD.Print("Adding level " + level.Name);
			LevelButton levelButton = this.LevelButton.Instantiate<LevelButton>();
			levelButton.LevelName = level.Name;
			levelButton.Pressed += () =>
			{
				LevelLoader.Instance.LoadLevel(level);
			};
			
			this.AddChild(levelButton);
		}
	}
}

