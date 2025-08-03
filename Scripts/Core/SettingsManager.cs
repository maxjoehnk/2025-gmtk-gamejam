using Godot;

namespace gmtkgamejam.Scripts.Core;

public partial class SettingsManager : Node
{
	private const int MasterBus = 0;
	private const int EffectsBus = 1;
	private const int MusicBus = 2;

	public override void _Ready()
	{
		LoadSettings();
	}

	public static double MainVolume
	{
		get => AudioServer.Singleton.GetBusVolumeLinear(MasterBus);
		set => AudioServer.Singleton.SetBusVolumeLinear(MasterBus, (float)value);
	}

	public static double EffectsVolume
	{
		get => AudioServer.Singleton.GetBusVolumeLinear(EffectsBus);
		set => AudioServer.Singleton.SetBusVolumeLinear(EffectsBus, (float)value);
	}
	
	public static double MusicVolume
	{
		get => AudioServer.Singleton.GetBusVolumeLinear(MusicBus);
		set => AudioServer.Singleton.SetBusVolumeLinear(MusicBus, (float)value);
	}

	public static void LoadSettings()
	{
		if (!FileAccess.FileExists("user://settings.tres"))
		{
			SaveSettings();
			return;
		}
		
		Settings? settings = ResourceLoader.Load<Settings>("user://settings.tres");
		if (settings == null)
		{
			SaveSettings();
			return;
		}
		
		MainVolume = settings.MainVolume;
		EffectsVolume = settings.EffectsVolume;
		MusicVolume = settings.MusicVolume;
	}
	
	public static void SaveSettings()
	{
		Settings settings = new()
		{
			MainVolume = MainVolume,
			EffectsVolume = EffectsVolume,
			MusicVolume = MusicVolume
		};
		
		ResourceSaver.Save(settings, "user://settings.tres");
	}
}