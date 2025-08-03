using Godot;
using System;
using gmtkgamejam.Core;
using gmtkgamejam.Scripts.Core;

public partial class Settings : Panel
{
	private Slider MasterVolumeSlider => GetNode<Slider>("VBoxContainer/VBoxContainer/MainVolume/HSlider");
	private Slider EffectsVolumeSlider => GetNode<Slider>("VBoxContainer/VBoxContainer/EffectsVolume/HSlider");
	private Slider MusicVolumeSlider => GetNode<Slider>("VBoxContainer/VBoxContainer/MusicVolume/HSlider");
	
	public override void _Ready()
	{
		this.MasterVolumeSlider.Value = SettingsManager.MainVolume;
		this.MasterVolumeSlider.ValueChanged += this.OnMasterVolumeChanged;
		this.EffectsVolumeSlider.Value = SettingsManager.EffectsVolume;
		this.EffectsVolumeSlider.ValueChanged += this.OnEffectsVolumeChanged;
		this.MusicVolumeSlider.Value = SettingsManager.MusicVolume;
		this.MusicVolumeSlider.ValueChanged += this.OnMusicVolumeChanged;
	}

	private void OnMasterVolumeChanged(double value)
	{
		SettingsManager.MainVolume = value;
	}
	
	private void OnEffectsVolumeChanged(double value)
	{
		SettingsManager.EffectsVolume = value;
	}
	
	private void OnMusicVolumeChanged(double value)
	{
		SettingsManager.MusicVolume = value;
	}

	public void OnDeleteUserData()
	{
		UserDataManager.ClearUserData();
		LevelManager.Instance.ReloadUserData();
	}
	
	public void OnSaveSettings()
	{
		SettingsManager.SaveSettings();
		LevelManager.Instance.OpenMainMenu();
	}

	public void OnBackPressed()
	{
		SettingsManager.LoadSettings();
		LevelManager.Instance.OpenMainMenu();
	}
}
