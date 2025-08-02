using Godot;
using System;
using gmtkgamejam.Core;
using gmtkgamejam.Scripts.Core;

public partial class MainMenu : Panel
{
	private Button StartOrResumeLabel => GetNode<Button>("VBoxContainer/Start");

	public override void _Ready()
	{
		this.UpdateLabels();
	}

	public void OnExit()
	{
		this.GetTree().Quit();
	}

	public void OnLevelSelect()
	{
		LevelManager.Instance.OpenLevelSelector();
	}

	public void OnStart()
	{
		LevelManager.Instance.LoadCurrentLevel();
	}

	public void OnDeleteUserData()
	{
		UserDataManager.ClearUserData();
		LevelManager.Instance.ReloadUserData();
		this.UpdateLabels();
	}

	private void UpdateLabels()
	{
		bool hasCompletedALevel = LevelManager.Instance.HasCompletedALevel();
		this.StartOrResumeLabel.Text = hasCompletedALevel ? "Resume" : "Start";
	}
}
