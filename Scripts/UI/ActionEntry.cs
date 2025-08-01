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
        Label? titleLabel = this.GetNode<Label>("ActionName");
        Button? deleteButton = this.GetNode<Button>("RemoveButton");
        Button? tickButton = this.GetNode<Button>("TicksButton");

        titleLabel.Text = this.Action.Title;

        tickButton.GuiInput += (InputEvent @event) =>
        {
            if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
            {
                if (mouseEvent.ButtonIndex == MouseButton.Left)
                    this.Action.Ticks++;
                else if (mouseEvent.ButtonIndex == MouseButton.Right)
                    this.Action.Ticks = System.Math.Max(0, this.Action.Ticks - 1);

                if (this.Action.Ticks is > 5 or < 1)
                {
                    this.Action.Ticks = 1;
                }

                tickButton.Text = this.Action.Ticks.ToString();
                this.EmitSignalActionChanged();
            }
        };

        deleteButton.Pressed += this.QueueFree;
    }

    public void UpdateTickDisplay()
    {
        GetNode<Button>("TicksButton").Text = Action.Ticks.ToString();
    }

    public override Variant _GetDragData(Vector2 position)
    {
        var preview = Duplicate() as Control;
        if(preview != null)
        {
            preview.Modulate = new Color(1, 1, 1, 0.5f);
            SetDragPreview(preview);
        }
        return this;
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        var entry = data.AsGodotObject() as ActionEntry;
        return entry != null && entry != this && entry.GetParent() == GetParent();
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        var dragged = data.AsGodotObject() as ActionEntry;
        if (dragged == null || dragged == this || dragged.GetParent() != GetParent())
            return;

        Node container = GetParent();
        Godot.Collections.Array<Node> children = container.GetChildren();
        int currentIndex = children.IndexOf(this);
        int draggedIndex = children.IndexOf(dragged);

        float halfHeight = Size.Y / 2.0f;
        bool dropBelow = atPosition.Y > halfHeight;
        int newIndex = dropBelow ? currentIndex + 1 : currentIndex;

        if (draggedIndex < newIndex)
            newIndex -= 1;

        container.MoveChild(dragged, newIndex);
    }
}
