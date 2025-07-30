using System.Linq;
using Godot;
using gmtkgamejam.Core;
using gmtkgamejam.Scenes;
using Godot.Collections;

public partial class Game : Node2D
{
    private ActionPane ActionPane => GetNode<ActionPane>("ActionPane");
    public Player Player => GetNode<Player>("Player");

    private ActionPlayer ActionPlayer => GetNode<ActionPlayer>("ActionPlayer");
    
    public Array<Action> Actions { get; set; }

    public override void _Ready()
    {
        base._Ready();
        ActionPane.ActionsUpdated += OnUpdateActions;
    }

    private void OnUpdateActions(Array<Action> actions)
    {
        Actions = actions;
    }

    public void OnPlayPressed()
    {
        GD.Print("Play");
        ActionPlayer.Play(Actions);
    }

    public void OnResetPressed()
    {
        GD.Print("Reset");
        ActionPlayer.Reset();
    }
}
