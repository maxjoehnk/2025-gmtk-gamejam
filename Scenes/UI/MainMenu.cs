using Godot;
using System;
using gmtkgamejam.Core;

public partial class MainMenu : Panel
{
	public void OnExit()
	{
		GetTree().Quit();
	}

	public void OnLevelSelect()
	{
		LevelLoader.Instance.OpenLevelSelector();
	}

	public void OnStart()
	{
		GD.Print("WIP: Track Progress");
	}
}
