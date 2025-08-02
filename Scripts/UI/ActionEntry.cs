using Godot;
using gmtkgamejam.Core;

public partial class ActionEntry : HBoxContainer
{
	[Export] public Action Action { get; set; }

	[Signal]
	public delegate void ActionChangedEventHandler();

	private Label TitleLabel => this.GetNode<Label>("ActionName");
	private Button DeleteButton => this.GetNode<Button>("RemoveButton");
	private Label TicksLabel => this.GetNode<Label>("HBoxContainer/TicksButton");

	public override void _Ready()
	{
		this.TitleLabel.Text = this.Action.Title;
		this.DeleteButton.Pressed += this.QueueFree;
	}

	public void OnAddTick()
	{
		this.Action.Ticks++;
		this.UpdateTicks();
	}

	public void OnRemoveTick()
	{
		if (this.Action.Ticks is 1)
		{
			return;
		}

		this.Action.Ticks--;
		this.UpdateTicks();
	}

	private void UpdateTicks()
	{
		this.UpdateTickDisplay();
		this.EmitSignalActionChanged();
	}

	public void UpdateTickDisplay()
	{
		this.TicksLabel.Text = this.Action.Ticks.ToString();
	}

	public override Variant _GetDragData(Vector2 position)
	{
		if (this.Duplicate() is not Control preview)
		{
			return this;
		}

		preview.Modulate = new Color(1, 1, 1, 0.5f);
		this.SetDragPreview(preview);
		return this;
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		var entry = data.AsGodotObject() as ActionEntry;
		return entry != null && entry != this && entry.GetParent() == this.GetParent();
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		if (data.AsGodotObject() is not ActionEntry dragged || dragged == this || dragged.GetParent() != this.GetParent())
		{
			return;
		}

		Node container = this.GetParent();
		Godot.Collections.Array<Node> children = container.GetChildren();
		int currentIndex = children.IndexOf(this);
		int draggedIndex = children.IndexOf(dragged);

		float halfHeight = this.Size.Y / 2.0f;
		bool dropBelow = atPosition.Y > halfHeight;
		int newIndex = dropBelow ? currentIndex + 1 : currentIndex;

		if (draggedIndex < newIndex)
		{
			newIndex -= 1;
		}

		container.MoveChild(dragged, newIndex);
	}
}