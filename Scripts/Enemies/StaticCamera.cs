using Godot;
using gmtkgamejam.Scenes.Enemies;
using gmtkgamejam.Scripts.Core;

public partial class StaticCamera : Enemy, ISwitchable, IResettable
{
	private bool state = true;

	[Export]
	public bool IsActive { get; set; } = true;

	public bool State
	{
		get => this.state;
		set
		{
			this.state = value;
			this.Update();
		}
	}

	public Node2D CameraArea => this.GetNode<Node2D>("CameraArea");

	public Area2D DetectionArea => this.GetNode<Area2D>("Area2D");

	public override void _Ready()
	{
		this.Reset();
	}

	private void Update()
	{
		if (!this.State)
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
		this.State = !this.State;
		this.Update();
	}

	public void Reset()
	{
		this.State = this.IsActive;
		this.Update();
	}
}
