using gmtkgamejam.Core;
using Godot;

public partial class View : Camera2D
{
	[Export] public Vector2 MinDistanceToViewBorderX { get; set; } = new Vector2(448, 256);
	[Export] public Vector2 MinDistanceToViewBorderY { get; set; } = new Vector2(256, 256);
	[Export] public float LowpassFactor { get; set; } = 0.9f;
	[Export] public float ManualCameraSpeed { get; set; } = 1000;

	private double maxWidth = 1920;
	private double maxHeight = 1080;

	private Vector2 nextPosition;

	public override void _Ready()
	{
		this.nextPosition = this.Position;
	}

	public override void _Process(double delta)
	{
		GameState state = this.GetParent<Game>().CurrentGameState;
		switch (state)
		{
			case GameState.Playing:
				this.HandlePlaying();
				break;
			case GameState.Prepare:
			case GameState.Previewing:
				this.HandlePrepare(delta);
				break;
			default:
				this.nextPosition = this.Position;
				break;
		}

		this.Position = this.Position * this.LowpassFactor + (1f - this.LowpassFactor) * this.nextPosition;
	}

	private float DistanceToRange(double val, double min, double max)
	{
		return (float)(val < min ? val - min : val > max ? val - max : 0);
	}

	private void HandlePlaying()
	{
		Player player = this.GetParent().GetNode<Player>("Player");
		Vector2 playerPos = player.Position;
		Vector2 cameraPos = this.Offset + this.Position;
		double upperEdge = cameraPos.Y - this.maxHeight / 2 + this.MinDistanceToViewBorderY.Y;
		double lowerEdge = cameraPos.Y + this.maxHeight / 2 - this.MinDistanceToViewBorderY.X;
		double leftEdge = cameraPos.X - this.maxWidth / 2 + this.MinDistanceToViewBorderX.X;
		double rightEdge = cameraPos.X + this.maxWidth / 2 - this.MinDistanceToViewBorderX.Y;
		this.nextPosition = this.Position + new Vector2(this.DistanceToRange(playerPos.X, leftEdge, rightEdge),
			this.DistanceToRange(playerPos.Y, upperEdge, lowerEdge));
	}

	private void HandlePrepare(double delta)
	{
		if (Input.IsActionPressed("Camera_Up"))
		{
			this.nextPosition += new Vector2(0, -this.ManualCameraSpeed * (float)delta);
		}

		if (Input.IsActionPressed("Camera_Down"))
		{
			this.nextPosition += new Vector2(0, this.ManualCameraSpeed * (float)delta);
		}

		if (Input.IsActionPressed("Camera_Left"))
		{
			this.nextPosition += new Vector2(-this.ManualCameraSpeed * (float)delta, 0);
		}

		if (Input.IsActionPressed("Camera_Right"))
		{
			this.nextPosition += new Vector2(this.ManualCameraSpeed * (float)delta, 0);
		}
	}
}