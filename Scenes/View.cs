using gmtkgamejam.Core;
using Godot;
using System;

public partial class View : Camera2D
{
  [Export] public double minDistanceToViewBorder { get; set; } = 192;
  [Export] public float lowpassFactor { get; set; } = 0.9f;
  [Export] public float manualCameraSpeed { get; set; } = 1000;

  private double maxWidth = 1920;
  private double maxHeight = 1080;

  Vector2 nextPosition;

  public override void _Ready()
  {
    nextPosition = this.Position;
  }

  public override void _Process(double delta)
  {
    GameState state = this.GetParent<Game>().CurrentGameState;
    switch(state)
    {
      case GameState.Playing:
        handlePlaying();
        break;
      case GameState.Prepare:
        handlePrepare(delta);
        break;
      default:
        nextPosition = this.Position;
        break;
    }
    this.Position = this.Position* lowpassFactor + (1f - lowpassFactor) * nextPosition;
  }
  private float distanceToRange(double val, double min, double max)
  {
    return (float)(val < min ? val - min : val > max ? val - max : 0);
  }

  private void handlePlaying()
  {
    Player Player = this.GetParent().GetNode<Player>("Player");
    Vector2 playerPos = Player.Position;
    Vector2 cameraPos = this.Offset + this.Position;
    double upperEdge = cameraPos.Y - maxHeight / 2 + minDistanceToViewBorder;
    double lowerEdge = cameraPos.Y + maxHeight / 2 - minDistanceToViewBorder;
    double leftEdge = cameraPos.X - maxWidth / 2 + minDistanceToViewBorder;
    double rightEdge = cameraPos.X + maxWidth / 2 - minDistanceToViewBorder;
    nextPosition = this.Position + new Vector2(distanceToRange((double)playerPos.X, leftEdge, rightEdge), distanceToRange((double)playerPos.Y, upperEdge, lowerEdge));
  }

  private void handlePrepare(double delta)
  {
    if(Input.IsActionPressed("Camera_Up"))
      nextPosition += new Vector2(0, -this.manualCameraSpeed * (float)delta);
    if(Input.IsActionPressed("Camera_Down"))
      nextPosition += new Vector2(0, this.manualCameraSpeed * (float)delta);
    if(Input.IsActionPressed("Camera_Left"))
      nextPosition += new Vector2(-this.manualCameraSpeed * (float)delta, 0);
    if(Input.IsActionPressed("Camera_Right"))
      nextPosition += new Vector2(this.manualCameraSpeed * (float)delta, 0);
  }
}
