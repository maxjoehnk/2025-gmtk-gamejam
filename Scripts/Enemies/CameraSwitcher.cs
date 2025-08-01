using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using gmtkgamejam.Scenes;

public partial class CameraSwitcher : Node2D
{
	private ActionPlayer Playback => ActionPlayer.Get(this);

	[Export] public int Ticks { get; set; } = 1;

	private List<StaticCamera> Cameras = [];
	
	public override void _Ready()
	{
		this.Cameras = this.GetChildren().OfType<StaticCamera>().ToList();
		this.Playback.Ticked += OnTick;
		this.OnTick(0);
	}

	private void OnTick(int tick)
	{
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
