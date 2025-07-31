using System.Linq;
using Godot;
using gmtkgamejam.Core;
using gmtkgamejam.Scenes;
using Godot.Collections;

public partial class Game : Node2D
{
	private ActionPane ActionPane => GetNode<ActionPane>("ActionPane");
	private LoopPane LoopPane => GetNode<LoopPane>("LoopPane");
	
	public Map Map => GetNode<Map>("CurrentMap");
	public Player Player => GetNode<Player>("Player");

	private ActionPlayer ActionPlayer => GetNode<ActionPlayer>("ActionPlayer");
	
	public Array<Action> Actions { get; set; }

	public override void _Ready()
	{
		base._Ready();
		Player.Position = Map.SpawnPosition;
		LoopPane.ActionsUpdated += OnUpdateActions;
	}

	private void OnUpdateActions(Array<Action> actions)
	{
		Actions = actions;
	}

	public void OnPlayPressed()
	{
		GD.Print("Play");
		Player.Position = Map.SpawnPosition;
		ActionPlayer.Play(Actions);
	}

	public void OnResetPressed()
	{
		GD.Print("Reset");
		Player.Position = Map.SpawnPosition;
		ActionPlayer.Reset();
	}
}
