using Godot;
using System;
using gmtkgamejam.Scripts.Core;
using Godot.Collections;

public partial class Switch : Node2D, IInteractable
{
	[Export] public Array<NodePath> Targets { get; set; } = new();
	
	public void Interact()
	{
		foreach (NodePath nodePath in Targets)
		{
			ISwitchable switchable = GetNode<ISwitchable>(nodePath);
			switchable.Toggle();
		}
	}
}
