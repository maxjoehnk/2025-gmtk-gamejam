using gmtkgamejam.Core;
using Godot;
using System.Linq;
using Godot.Collections;

public partial class ActionPane : PanelContainer
{
	private PackedScene MovementActionScene => GD.Load<PackedScene>("res://Scenes/UI/Actions/MovementAction.tscn");
	private PackedScene InteractActionScene => GD.Load<PackedScene>("res://Scenes/UI/Actions/InteractAction.tscn");
	private PackedScene WaitActionScene => GD.Load<PackedScene>("res://Scenes/UI/Actions/WaitAction.tscn");
	private PackedScene ActionEntryScene => GD.Load<PackedScene>("res://Scenes/UI/ActionMenu/ActionEntry.tscn");
	
	private VBoxContainer ActionList => GetNode<VBoxContainer>("HBoxContainer/MainVBox/ScrollContainer/ActionList");
	private Button AddActionButton => GetNode<Button>("HBoxContainer/MainVBox/AddActionButton");
	private PopupMenu ActionPopupMenu => GetNode<PopupMenu>("HBoxContainer/MainVBox/ActionPopupMenu");

	// ActionPaneUI Buttons
	private TextureButton AddLeftButton => GetNode<TextureButton>("HBoxContainer/MainVBox/HBoxContainer/Left");
	private TextureButton AddUpButton => GetNode<TextureButton>("HBoxContainer/MainVBox/HBoxContainer/Up");
	private TextureButton AddDownButton => GetNode<TextureButton>("HBoxContainer/MainVBox/HBoxContainer/Down");
	private TextureButton AddRightButton => GetNode<TextureButton>("HBoxContainer/MainVBox/HBoxContainer/Right");
	private TextureButton AddInteractButton => GetNode<TextureButton>("HBoxContainer/MainVBox/HBoxContainer/Interact");
	private TextureButton AddWaitButton => GetNode<TextureButton>("HBoxContainer/MainVBox/HBoxContainer/Wait");
	private Button CollapseButton => GetNode<Button>("HBoxContainer/CollapseButton");
	private VBoxContainer MainVBox => GetNode<VBoxContainer>("HBoxContainer/MainVBox");


	public Array<Action> Actions => new(ActionList.GetChildren().Cast<ActionEntry>().Select(a => a.Action));
	
	[Signal]
	public delegate void ActionsChangedEventHandler();

	public override void _Ready()
	{
		this.ActionList.ChildExitingTree += child =>
		{
			child.TreeExited += this.EmitSignalActionsChanged;
		};
		this.ActionList.ChildEnteredTree += child =>
		{
			if (child is ActionEntry entry)
			{
				entry.ActionChanged += this.EmitSignalActionsChanged;
			}
			this.EmitSignalActionsChanged();
		};
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.IsActionJustPressed("Add_MoveLeft"))
			OnLeftButton_Click();

		if (Input.IsActionJustPressed("Add_MoveUp"))
			OnUpButton_Click();

		if (Input.IsActionJustPressed("Add_MoveDown"))
			OnDownButton_Click();

		if (Input.IsActionJustPressed("Add_MoveRight"))
			OnRightButton_Click();

		if (Input.IsActionJustPressed("Add_Interact"))
			OnInteractButton_Click();

		if (Input.IsActionJustPressed("Add_Wait"))
			OnWaitButton_Click();
	}

	public void OnCollapseButton_Click(bool isCollapsed)
	{
		Texture2D collapsedTrue = GD.Load<Texture2D>("res://Assets/chevron-left.png");
		Texture2D collapsedFalse = GD.Load<Texture2D>("res://Assets/chevron-right.png");

		MainVBox.Visible = !isCollapsed;
		if (isCollapsed)
		{
			CollapseButton.Icon = collapsedFalse;
			this.SelfModulate = new Color(255, 255, 255, 0);
		}
		else
		{
			CollapseButton.Icon = collapsedTrue;
			this.SelfModulate = new Color("#ffffff");
		}
		GD.Print($"ActionPane: Collapsing: {isCollapsed}");
	}

	public void OnLeftButton_Click()
	{
		AddMoveAction(MoveDirection.Left);
	}

	public void OnUpButton_Click()
	{
		AddMoveAction(MoveDirection.Up);
	}

	public void OnDownButton_Click()
	{
		AddMoveAction(MoveDirection.Down);
	}

	public void OnRightButton_Click()
	{
		AddMoveAction(MoveDirection.Right);
	}

	public void OnInteractButton_Click()
	{
		AddInteractAction();
	}

	public void OnWaitButton_Click()
	{
		AddWaitAction();
	}
	
	public void OnDeleteButton_Click() 
	{
		foreach (Node child in ActionList.GetChildren())
		{
			child.QueueFree();
		}
	}

	private void AddAction(Action action)
	{
		ActionEntry entry = ActionEntryScene.Instantiate<ActionEntry>();
		entry.Action = action;

		int childCount = ActionList.GetChildCount();

		if (childCount > 0)
		{
			Node lastChild = ActionList.GetChild(childCount - 1);

			if (lastChild is ActionEntry lastEntry && lastEntry.Action.Title == action.Title)
			{
				lastChild.QueueFree();
				GD.Print($"{entry.Action.Name} has {entry.Action.Ticks}");
				entry.Action.Ticks = lastEntry.Action.Ticks + 1;
				entry.UpdateTickDisplay();
			}
		}

		ActionList.AddChild(entry);
	}

	//private void AddAction(Action action)
	//{
	//	ActionEntry entry = ActionEntryScene.Instantiate<ActionEntry>();
	//	entry.Action = action;

	//	GD.Print($"Added {entry.Action.Title}, with {entry.Action.Ticks} Ticks.");

	//	ActionList.AddChild(entry);
	//}

	//private void AddAction(Action action)
	//{
	//	int childCount = ActionList.GetChildCount();

	//	ActionEntry newEntry = ActionEntryScene.Instantiate<ActionEntry>();
	//	newEntry.Action = action;

	//	if (childCount > 0)
	//	{
	//		Node lastChild = ActionList.GetChild(childCount - 1);

	//		if (lastChild is ActionEntry lastEntry && lastEntry.Action.Title == action.Title)
	//		{
	//			lastChild.QueueFree();

	//			newEntry.Action.Ticks = lastEntry.Action.Ticks + 1;
	//			lastEntry.Action.Ticks += action.Ticks;
	//			lastEntry.UpdateTickDisplay();
	//			GD.Print($"Updated {lastEntry.Action.Title}, now {lastEntry.Action.Ticks} Ticks.");
	//			return;
	//		}
	//	}

	//	newEntry.UpdateTickDisplay();
	//	ActionList.AddChild(newEntry);

	//	GD.Print($"Added {newEntry.Action.Title}, with {newEntry.Action.Ticks} Ticks.");
	//}

	private void AddMoveAction(MoveDirection dir)
	{
		var action = MovementActionScene.Instantiate<MoveAction>();
		action.Direction = dir;
		AddAction(action);
	}

	private void AddInteractAction()
	{
		var action = InteractActionScene.Instantiate<InteractAction>();
		AddAction(action);
	}

	private void AddWaitAction()
	{
		var action = WaitActionScene.Instantiate<WaitAction>();
		AddAction(action);
	}
}
