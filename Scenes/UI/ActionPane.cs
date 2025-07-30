using System.Linq;
using gmtkgamejam.Core;
using Godot;
using Godot.Collections;

public partial class ActionPane : PanelContainer
{
    private PackedScene MovementActionScene => GD.Load<PackedScene>("res://Scenes/UI/Actions/MovementAction.tscn");
    private PackedScene InteractActionScene => GD.Load<PackedScene>("res://Scenes/UI/Actions/InteractAction.tscn");
    private PackedScene WaitActionScene => GD.Load<PackedScene>("res://Scenes/UI/Actions/WaitAction.tscn");
    
    private BoxContainer ActionListContainer => GetNode<BoxContainer>("HBoxContainer/ActionList");
    
    private Button MoveLeftButton => GetNode<Button>("HBoxContainer/MoveLeftAction");
    private Button MoveRightButton => GetNode<Button>("HBoxContainer/MoveRightAction");
    private Button MoveUpButton => GetNode<Button>("HBoxContainer/MoveUpAction");
    private Button MoveDownButton => GetNode<Button>("HBoxContainer/MoveDownAction");
    private Button InteractButton => GetNode<Button>("HBoxContainer/InteractAction");

    [Signal]
    public delegate void ActionsUpdatedEventHandler(Array<Action> actions);

    public override void _Ready()
    {
        MoveLeftButton.ButtonUp += OnAddMoveLeftAction;
        MoveRightButton.ButtonUp += OnAddMoveRightAction;
        MoveUpButton.ButtonUp += OnAddMoveUpAction;
        MoveDownButton.ButtonUp += OnAddMoveDownAction;
        InteractButton.ButtonUp += OnAddInteractAction;
    }

    private void AddAction(Action action)
    {
        ActionListContainer.AddChild(action);
        var actions = new Array<Action>(ActionListContainer.GetChildren().Cast<Action>());
        EmitSignal(SignalName.ActionsUpdated, actions);
    }
    
    private void OnAddMoveUpAction()
    {
        var action = MovementActionScene.Instantiate<MoveAction>();
        action.Direction = MoveDirection.Up;
        AddAction(action);
    }

    private void OnAddMoveDownAction()
    {
        var action = MovementActionScene.Instantiate<MoveAction>();
        action.Direction = MoveDirection.Down;
        AddAction(action);
    }

    private void OnAddMoveLeftAction()
    {
        var action = MovementActionScene.Instantiate<MoveAction>();
        action.Direction = MoveDirection.Left;
        AddAction(action);
    }

    private void OnAddMoveRightAction()
    {
        var action = MovementActionScene.Instantiate<MoveAction>();
        action.Direction = MoveDirection.Right;
        AddAction(action);
    }
    
    private void OnAddInteractAction()
    {
        var action = InteractActionScene.Instantiate<InteractAction>();
        AddAction(action);
    }
}