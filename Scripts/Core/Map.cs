using gmtkgamejam.Core;
using Godot;
using gmtkgamejam.Scenes.Enemies;
using gmtkgamejam.Scripts.Core;
using Godot.Collections;

public partial class Map : Node
{
	private PackedScene LevelName => GD.Load<PackedScene>("res://Scenes/UI/LevelName.tscn");
	
	public Vector2 SpawnPosition => this.GetNode<Node2D>("TileMapLayer/Spawn").GlobalPosition;
	public Game Game => this.GetNode<Game>("Game");

	[Export] public int GoldMedalTicks { get; set; }

	[Export] public int SilverMedalTicks { get; set; }

	[Export] public int BronzeMedalTicks { get; set; }

	[Signal]
	public delegate void PlayerLostEventHandler();

	[Signal]
	public delegate void PlayerWonEventHandler();

	public override void _Ready()
	{
		Array<Node> enemies = this.GetTree().GetNodesInGroup(Groups.Enemies);
		foreach (Node enemy in enemies)
		{
			GD.Print($"Connecting catched player signal from {enemy} ({enemy.Name})");
			enemy.Connect(nameof(Enemy.CatchedPlayer), Callable.From(this.OnPlayerCaught));
		}

		Array<Node> exits = this.GetTree().GetNodesInGroup(Groups.LevelExits);
		foreach (Node exit in exits)
		{
			exit.Connect(nameof(LevelGoal.PlayerReachedGoal), Callable.From(this.OnPlayerWon));
		}

		this.Game.SpawnPosition = this.SpawnPosition;
		this.Game.Respawn();
		this.Game.Show();

		this.ShowLevelName();
	}

	private void ShowLevelName()
	{
		LevelName levelName = this.LevelName.Instantiate<LevelName>();
		this.AddChild(levelName);
	}

	private void OnPlayerCaught()
	{
		if (this.Game.CurrentGameState != GameState.Playing)
		{
			return;
		}

		GD.Print("Player caught");
		this.Game.OnPlayerLost();
	}

	public void OnPlayerWon()
	{
		GD.Print("Player won");
		if (this.Game.CurrentGameState != GameState.Playing)
		{
			return;
		}

		this.Game.OnPlayerWon(this.GoldMedalTicks, this.SilverMedalTicks, this.BronzeMedalTicks);
	}
}