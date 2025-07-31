using Godot;

namespace gmtkgamejam.Core;

public partial class MoveAction : Action
{
	public MoveDirection Direction { get; set; }
	public override string Title => "Move " + Direction;

	public override void _Ready()
	{
		var entryScene = GD.Load<PackedScene>("res://Scenes/UI/ActionEntry.tscn");
		var entry = entryScene.Instantiate<HBoxContainer>();
		AddChild(entry);

		// UI-Elemente holen
		var tickButton = entry.GetNode<Button>("TickButton");
		var titleLabel = entry.GetNode<Label>("TitleLabel");
		var deleteButton = entry.GetNode<Button>("DeleteButton");

		//SetTickButton(tickButton);
		titleLabel.Text = Title;

		deleteButton.Pressed += QueueFree;
	}

	public override void Act(Player player)
	{
		GD.Print("Move");
		player.Move(Direction);
	}
}
