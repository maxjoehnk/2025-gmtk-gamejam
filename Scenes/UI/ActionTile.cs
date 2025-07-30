using Godot;
using System;

public partial class ActionTile : Control
{
    private Label Label => GetNode<Label>("Label");
    
    [Export]
    public string Title { get; set; }

    public override void _Ready()
    {
        base._Ready();
        Label.Text = Title;
    }
}
