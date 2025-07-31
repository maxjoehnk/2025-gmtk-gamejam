using Godot;
using System;
using gmtkgamejam.Scenes;
using gmtkgamejam.Scenes.Enemies;

public partial class StaticCamera : Enemy
{
	private bool isActive = true;

	[Export]
	public bool IsActive
	{
		get => this.isActive;
		set
		{
			this.isActive = value;
			this.Update();
		}
	}

	public Node2D CameraArea => this.GetNode<Node2D>("CameraArea");
	
	public Area2D DetectionArea => this.GetNode<Area2D>("Area2D");

	public override void _Ready()
	{
		this.Update();
	}

	private void Update()
	{
		if (!this.IsActive)
		{
			this.CameraArea.Hide();
			this.DetectionArea.ProcessMode = ProcessModeEnum.Disabled;
		}
		else
		{
			this.CameraArea.Show();
			this.DetectionArea.ProcessMode = ProcessModeEnum.Inherit;
		}
	}
}
