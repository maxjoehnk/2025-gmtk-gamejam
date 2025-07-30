using Godot;
using gmtkgamejam.Core;
using Godot.Collections;

public partial class Game : Node2D
{
    private ActionPane ActionPane => GetNode<ActionPane>("ActionPane");
    
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
}
