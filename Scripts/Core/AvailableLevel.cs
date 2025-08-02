namespace gmtkgamejam.Scenes;

public class AvailableLevel
{
	public string Path { get; }

	public string Name => this.Path.Replace(".tscn", "");

	public bool IsUnlocked { get; set; }
	
	public AvailableLevel(string path)
	{
		this.Path = path;
	}
}