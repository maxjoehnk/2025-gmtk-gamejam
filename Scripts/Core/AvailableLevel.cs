using System.Text.RegularExpressions;

namespace gmtkgamejam.Scenes;

public partial class AvailableLevel
{
	private const int FileIndexOffset = 1000;
	
	public string Path { get; }

	public string Name { get; }

	public int LevelIndex { get; }

	public bool IsUnlocked { get; set; }
	
	public AvailableLevel(string path, int fileIndex)
	{
		this.Path = path;
		Match match = LevelNameRegex().Match(path);
		this.Name = match.Groups["Name"].Value;
		if (match.Groups["Id"].Success)
		{
			this.LevelIndex = int.Parse(match.Groups["Id"].Value);
		}
		else
		{
			this.LevelIndex = FileIndexOffset + fileIndex;
		}
	}

	[GeneratedRegex("((?<Id>[0-9]+)_)?(?<Name>.*).tscn", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
	private static partial Regex LevelNameRegex();

}
