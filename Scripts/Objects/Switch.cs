using Godot;
using System;
using gmtkgamejam.Scenes;
using gmtkgamejam.Scripts.Core;
using Godot.Collections;

public partial class Switch : Node2D, IInteractable
{
	[Export] public Array<NodePath> Targets { get; set; } = [];
	private AnimatedSprite2D Sprite => this.GetNode<AnimatedSprite2D>("Switch");
	
	public void Interact(Player player)
	{
		this.Sprite.SpeedScale = (float)ActionPlayer.Instance.PlaybackSpeed;
		this.Sprite.Play();
		foreach (NodePath nodePath in this.Targets)
		{
			ISwitchable switchable = this.GetNode<ISwitchable>(nodePath);
			switchable.Toggle();
		}
	}
}
