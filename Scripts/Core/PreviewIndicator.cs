using Godot;
using System;
using gmtkgamejam.Scripts.Core;

public partial class PreviewIndicator : CharacterBody2D
{
	private RayCast2D RayCast => this.GetNode<RayCast2D>("RayCast2D");
	
	public IInteractable? GetInteractableElement()
	{
		this.RayCast.ForceRaycastUpdate();
		GodotObject collision = this.RayCast.GetCollider();
		if (collision is IInteractable interactable)
		{
			return interactable;
		}

		return null;
	}
}
