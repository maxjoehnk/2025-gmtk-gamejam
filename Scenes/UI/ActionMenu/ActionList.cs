using Godot;
using System;

public partial class ActionList : VBoxContainer
{
    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return data.AsGodotObject() is Control;
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        var dragged = data.AsGodotObject() as Control;
        if (dragged == null || dragged.GetParent() != this)
            return;

        int insertIndex = 0;
        foreach (var child in GetChildren())
        {
            if (child is Control control && atPosition.Y > control.Position.Y + control.Size.Y / 2.0)
                insertIndex++;
        }

        var currentIndex = GetChildren().IndexOf(dragged);
        if (currentIndex < insertIndex)
            insertIndex -= 1;

        MoveChild(dragged, insertIndex);
    }
}
