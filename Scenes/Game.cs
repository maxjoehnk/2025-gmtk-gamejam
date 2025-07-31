using System.Linq;
using Godot;
using gmtkgamejam.Core;
using gmtkgamejam.Scenes;
using Godot.Collections;

public partial class Game : Node2D
{
    private ActionPane ActionPane => GetNode<ActionPane>("ActionPane");
    public Map Map => GetNode<Map>("CurrentMap");
    public Player Player => GetNode<Player>("Player");

    private ActionPlayer ActionPlayer => GetNode<ActionPlayer>("ActionPlayer");

    public override void _Ready()
    {
        base._Ready();
        Player.Position = Map.SpawnPosition;
    }

    public void OnPlayPressed()
    {
        GD.Print("Play");
        Player.Position = Map.SpawnPosition;
        ActionPlayer.Play(ActionPane.Actions);
    }

    public void OnResetPressed()
    {
        GD.Print("Reset");
        Player.Position = Map.SpawnPosition;
        ActionPlayer.Reset();
    }
}
