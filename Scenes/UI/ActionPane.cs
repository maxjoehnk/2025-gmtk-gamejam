using gmtkgamejam.Core;
using Godot;
using Godot.Collections;

public partial class ActionPane : PanelContainer
{
    private PackedScene ActionTileScene => GD.Load<PackedScene>("res://Scenes/UI/ActionTile.tscn");
    private BoxContainer ActionListContainer => GetNode<BoxContainer>("HBoxContainer/ActionList");
    
    private Button MoveLeftButton => GetNode<Button>("HBoxContainer/MoveLeftAction");
    private Button MoveRightButton => GetNode<Button>("HBoxContainer/MoveRightAction");
    private Button MoveUpButton => GetNode<Button>("HBoxContainer/MoveUpAction");
    private Button MoveDownButton => GetNode<Button>("HBoxContainer/MoveDownAction");
    private Button InteractButton => GetNode<Button>("HBoxContainer/InteractAction");
    
    private Array<Action> Actions { get; set; } = new();

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
        this.Actions.Add(action);
        EmitSignal(SignalName.ActionsUpdated, this.Actions);
        var tile = ActionTileScene.Instantiate<ActionTile>();
        tile.Title = action.Title;
        ActionListContainer.AddChild(tile);
    }
    
    private void OnAddMoveUpAction()
    {
        Action action = new MoveAction()
        {
            Direction = MoveDirection.Up,
        };
        AddAction(action);
    }

    private void OnAddMoveDownAction()
    {
        Action action = new MoveAction()
        {
            Direction = MoveDirection.Down,
        };
        AddAction(action);
    }

    private void OnAddMoveLeftAction()
    {
        Action action = new MoveAction()
        {
            Direction = MoveDirection.Left,
        };
        AddAction(action);
    }

    private void OnAddMoveRightAction()
    {
        Action action = new MoveAction()
        {
            Direction = MoveDirection.Right,
        };
        AddAction(action);
    }
    
    private void OnAddInteractAction()
    {
        Action action = new InteractAction();
        AddAction(action);
    }
}