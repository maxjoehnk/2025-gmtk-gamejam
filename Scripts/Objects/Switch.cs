using Godot;
using System;
using gmtkgamejam.Scenes;
using gmtkgamejam.Scripts.Core;
using Godot.Collections;

public partial class Switch : Node2D, IInteractable
{
	[Export] public Array<NodePath> Targets { get; set; } = new();
	private AnimatedSprite2D Sprite => this.GetNode<AnimatedSprite2D>("Switch");
	
	public void Interact()
	{
		this.Sprite.SpeedScale = (float)ActionPlayer.Instance.PlaybackSpeed;
		this.Sprite.Play();
		foreach (NodePath nodePath in Targets)
		{
			ISwitchable switchable = GetNode<ISwitchable>(nodePath);
			switchable.Toggle();
		}
	}
}
