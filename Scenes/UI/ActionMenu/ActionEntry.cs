using Godot;
using gmtkgamejam.Core;

public partial class ActionEntry : HBoxContainer
{
    [Export]
    public Action Action { get; set; }

    [Signal]
    public delegate void ActionChangedEventHandler();
    
    public override void _Ready()
    {
        Button? tickButton = this.GetNode<Button>("TicksButton");
        Label? titleLabel = this.GetNode<Label>("ActionName");
        Button? deleteButton = this.GetNode<Button>("RemoveButton");

        titleLabel.Text = this.Action.Title;
        tickButton.Pressed += () =>
        {
            if (this.Action.Ticks is >= 5 or < 1)
            {
                this.Action.Ticks = 0;
            }

            this.Action.Ticks++;

            tickButton.Text = this.Action.Ticks.ToString();
            this.EmitSignalActionChanged();
        };

        deleteButton.Pressed += this.QueueFree;
    }
}
