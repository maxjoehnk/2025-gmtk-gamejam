using Godot;

namespace gmtkgamejam.Scripts.Core;

public partial class UserDataManager : GodotObject
{
	public static void StoreFinishedLevel(int index)
	{
		GD.Print("Storing current level index: " + index);
		
		PlayState playState = new();
		playState.LastPlayedLevelIndex = index;
		ResourceSaver.Save(playState, "user://user_data.tres");
	}
	
	public static int? LoadFinishedLevel()
	{
		if (!FileAccess.FileExists("user://user_data.tres"))
		{
			return null;
		}
		PlayState? playState = ResourceLoader.Load<PlayState>("user://user_data.tres");
		if (playState == null)
		{
			return null;
		}
		
		return playState.LastPlayedLevelIndex == -1 ? null : playState.LastPlayedLevelIndex;
	}

	public static void ClearUserData()
	{
		ResourceSaver.Save(new PlayState(), "user://user_data.tres");
	}
}