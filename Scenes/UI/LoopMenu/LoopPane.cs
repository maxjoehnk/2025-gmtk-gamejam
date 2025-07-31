using gmtkgamejam.Core;
using Godot;
using System.Linq;
using Godot.Collections;

public partial class LoopPane : PanelContainer
{
    private PackedScene MovementActionScene => GD.Load<PackedScene>("res://Scenes/UI/Actions/MovementAction.tscn");
    private PackedScene InteractActionScene => GD.Load<PackedScene>("res://Scenes/UI/Actions/InteractAction.tscn");
    private PackedScene ActionEntryScene => GD.Load<PackedScene>("res://Scenes/UI/LoopMenu/ActionEntry.tscn");
    
    private VBoxContainer ActionList => GetNode<VBoxContainer>("MainVBox/ScrollContainer/ActionList");
    private Button AddActionButton => GetNode<Button>("MainVBox/AddActionButton");
    private PopupMenu ActionPopupMenu => GetNode<PopupMenu>("MainVBox/ActionPopupMenu");

    [Signal]
    public delegate void ActionsUpdatedEventHandler(Godot.Collections.Array<Action> actions);

    public Array<Action> Actions => new(ActionList.GetChildren().Cast<Action>());

    public Array<ActionEntry> ActionEntries => new(ActionList.GetChildren().Cast<ActionEntry>());

    public override void _Ready()
    {
        GD.Print("LoopPane ready");

        SetupAddActionButton();
    }

    private void SetupAddActionButton()
    {
        AddActionButton.Pressed += ShowActionPopupMenu;
        ActionPopupMenu.IdPressed += OnActionSelected;

        AddPopupMenuItems();
    }

    private void AddPopupMenuItems()
    {
        ActionPopupMenu.AddItem("Bewegen ↑", 0);
        ActionPopupMenu.AddItem("Bewegen ↓", 1);
        ActionPopupMenu.AddItem("Bewegen ←", 2);
        ActionPopupMenu.AddItem("Bewegen →", 3);
        ActionPopupMenu.AddItem("Interagieren", 4);
    }

    private void ShowActionPopupMenu()
    {
        var menuHeight = ActionPopupMenu.GetContentsMinimumSize().Y;
        var buttonPos = AddActionButton.GlobalPosition;

        ActionPopupMenu.Size = (Vector2I)(new Vector2(AddActionButton.Size.X, ActionPopupMenu.Size.Y));
        ActionPopupMenu.Position = (Vector2I)(buttonPos - new Vector2(0, menuHeight));
        ActionPopupMenu.Popup();
    }

    private void OnActionSelected(long id)
    {
        switch (id)
        {
            case 0: AddMoveAction(MoveDirection.Up); break;
            case 1: AddMoveAction(MoveDirection.Down); break;
            case 2: AddMoveAction(MoveDirection.Left); break;
            case 3: AddMoveAction(MoveDirection.Right); break;
            case 4: AddInteractAction(); break;
        }
    }

    private void AddAction(Action action)
    {
        ActionEntry entry = ActionEntryScene.Instantiate<ActionEntry>();
        entry.Action = action;

        GD.Print($"Added {entry.Action.Title}, with {entry.Action.Ticks} Ticks.");

        ActionList.AddChild(entry);
        EmitSignal(SignalName.ActionsUpdated, ActionEntries);
    }

    private void AddMoveAction(MoveDirection dir)
    {
        var action = MovementActionScene.Instantiate<MoveAction>();
        action.Direction = dir;
        AddAction(action);

        EmitActionListUpdated();
    }

    private void AddInteractAction()
    {
        var action = InteractActionScene.Instantiate<InteractAction>();
        AddAction(action);

        EmitActionListUpdated();
    }

    private void EmitActionListUpdated()
    {
        var actions = new Godot.Collections.Array<Action>(
            ActionEntries.Select(entry => entry.Action)
        );

        GD.Print(actions.Count);

        EmitSignal(SignalName.ActionsUpdated, actions);
    }
}
