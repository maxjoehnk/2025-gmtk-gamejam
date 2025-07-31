using Godot;
using System;

public partial class GuardPath : PathFollow2D
{
	private Guard Guard => GetChild<Guard>(0);
	
	public override void _Process(double delta)
	{
		this.Progress += (float) (Guard.Speed * delta);
	}
}
