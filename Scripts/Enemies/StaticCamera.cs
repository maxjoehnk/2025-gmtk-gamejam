using Godot;
using System;
using gmtkgamejam.Scenes;
using gmtkgamejam.Scenes.Enemies;
using gmtkgamejam.Scripts.Core;

public partial class StaticCamera : Enemy, ISwitchable, IResettable
{
	private bool isActive = true;
	private bool state = true;

	[Export]
	public bool IsActive
	{
		get => this.isActive;
		set
		{
			this.isActive = value;
			this.state = value;
			this.Update();
		}
	}

	public Node2D CameraArea => this.GetNode<Node2D>("CameraArea");
	
	public Area2D DetectionArea => this.GetNode<Area2D>("Area2D");

	public override void _Ready()
	{
		this.state = this.isActive;
		this.Update();
	}

	private void Update()
	{
		if (!this.state)
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

	public void Toggle()
	{
		this.state = !this.state;
		this.Update();
	}

	public void Reset()
	{
		this.state = this.IsActive;
		this.Update();
	}
}
