using Godot;
using System;

public partial class View : Camera2D
{
  private Player player => GetParent<Player>();

  public override void _Process(double _)
  {
    SetRotation(-player.Rotation);
  }
}
