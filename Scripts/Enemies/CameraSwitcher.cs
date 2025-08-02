using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using gmtkgamejam.Scripts.Core;

public partial class CameraSwitcher : Node2D, IClocked
{
	[Export] public int Ticks { get; set; } = 1;

	private List<StaticCamera> Cameras = [];
	
	public override void _Ready()
	{
		this.Cameras = this.GetChildren().OfType<StaticCamera>().ToList();
		GD.Print($"CameraSwitcher {this.Name} subscribed to ticks");
		
		this.OnTick(0);
	}

	public void OnTick(int tick)
	{
		GD.Print($"CameraSwitcher {this.Name} ticked {tick}");
		int cameraIndex = (tick / this.Ticks) % this.Cameras.Count;
		this.Cameras[cameraIndex].IsActive = true;
		foreach (StaticCamera camera in this.Cameras)
		{
			if (camera != this.Cameras[cameraIndex])
			{
				camera.IsActive = false;
			}
		}
	}
}
