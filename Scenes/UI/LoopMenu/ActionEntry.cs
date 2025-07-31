using Godot;
using System;
using gmtkgamejam.Core;

public partial class ActionEntry : HBoxContainer
{
	[Export]
	public gmtkgamejam.Core.Action Action { get; set; }

	public override void _Ready()
	{
		var tickButton = GetNode<Button>("TicksButton");
		var titleLabel = GetNode<Label>("ActionName");
		var deleteButton = GetNode<Button>("RemoveButton");

		titleLabel.Text = Action.Title;
		tickButton.Pressed += () =>
		{
			Action.Ticks++;
			if (Action.Ticks > 5)
			{
				Action.Ticks = 1;
			}

			tickButton.Text = Action.Ticks.ToString();
			GD.Print("Ticks: " + Action.Ticks);
		};

		deleteButton.Pressed += () => {
			QueueFree();
			// Eventuell LoopPane über das Entfernen informieren
		};
	}
}
