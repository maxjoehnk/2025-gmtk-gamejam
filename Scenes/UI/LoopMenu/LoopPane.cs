using gmtkgamejam.Core;
using Godot;
using System;
using System.Linq;

public partial class LoopPane : PanelContainer
{
	private VBoxContainer ActionList => GetNode<VBoxContainer>("MainVBox/ScrollContainer/ActionList");
	private Button AddActionButton => GetNode<Button>("MainVBox/AddActionButton");
	private PopupMenu ActionPopupMenu => GetNode<PopupMenu>("MainVBox/ActionPopupMenu");

	private PackedScene MovementActionScene = GD.Load<PackedScene>("res://Scenes/UI/Actions/MovementAction.tscn");
	private PackedScene InteractActionScene = GD.Load<PackedScene>("res://Scenes/UI/Actions/InteractAction.tscn");
	
	[Signal]
	public delegate void ActionsUpdatedEventHandler(Godot.Collections.Array<gmtkgamejam.Core.Action> actions);

	public override void _Ready()
	{
		GD.Print("LoopPane ready");

		AddActionButton.Pressed += () =>
		{
			var menuHeight = ActionPopupMenu.GetContentsMinimumSize().Y;
			var buttonPos = AddActionButton.GlobalPosition;

			ActionPopupMenu.Size = (Vector2I)(new Vector2(AddActionButton.Size.X, ActionPopupMenu.Size.Y));
			ActionPopupMenu.Position = (Vector2I)(buttonPos - new Vector2(0, menuHeight));
			ActionPopupMenu.Popup();
		}; // zeigt das Menü
		ActionPopupMenu.IdPressed += OnActionSelected;

		// Optionen hinzufügen (ID kannst du frei wählen)
		ActionPopupMenu.AddItem("Bewegen ↑", 0);
		ActionPopupMenu.AddItem("Bewegen ↓", 1);
		ActionPopupMenu.AddItem("Bewegen ←", 2);
		ActionPopupMenu.AddItem("Bewegen →", 3);
		ActionPopupMenu.AddItem("Interagieren", 4);
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

	private PackedScene ActionEntryScene = GD.Load<PackedScene>("res://Scenes/UI/LoopMenu/ActionEntry.tscn");

	private void AddMoveAction(MoveDirection dir)
	{
		var action = new MoveAction();
		action.Direction = dir;

		var entry = ActionEntryScene.Instantiate<ActionEntry>();
		entry.Action = action;

		ActionList.AddChild(entry);
		EmitActionListUpdated();
	}

	private void AddInteractAction()
	{
		var action = InteractActionScene.Instantiate<InteractAction>();
		ActionList.AddChild(action);
		EmitActionListUpdated();
	}

	private void EmitActionListUpdated()
	{
		var actions = new Godot.Collections.Array<gmtkgamejam.Core.Action>(ActionList.GetChildren().Cast<gmtkgamejam.Core.Action>());
		EmitSignal(SignalName.ActionsUpdated, actions);
	}
}
