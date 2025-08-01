using Godot;
using gmtkgamejam.Core;
using gmtkgamejam.Scenes;
using Vector2 = Godot.Vector2;
using static Godot.TextServer;

public partial class Player : CharacterBody2D
{
  private const int GridSize = 128;

  [Export] public float RotationSpeed { get; set; } = 360;

  private ActionPlayer Playback => ActionPlayer.Get(this);
  private Vector2 nextDirection = new Vector2(0, 0);
  private float nextRotation = 0;
  private float currentRotation = 0;
  private double executedTime = 0;
  private double lastExecutedRatio = 0;

  public void Move(MoveDirection direction)
  {
    nextDirection = direction.ToMovementVector();
    executedTime = 0;
    nextRotation = GetRotation(direction);

    KinematicCollision2D ? collision2D = MoveAndCollide(nextDirection, testOnly: true);
    if(collision2D != null)
    {
      nextDirection = new Vector2(0, 0);
      return;
    }

    lastExecutedRatio = 0;
  }

  public override void _Process(double delta)
  {
    if(nextDirection == new Vector2(0, 0))
      return;

    double lastExecutedTime = executedTime;
    executedTime += delta;

    float maxRotationPerUpdate = this.RotationSpeed* (float)delta / (float)this.Playback.TickDuration;
    currentRotation += limit(nextRotation - currentRotation, maxRotationPerUpdate);

    double baseRatio = Mathf.Min(1, this.executedTime / this.Playback.TickDuration);

    double lastRatioTranslation = Mathf.Sin(-Mathf.Pi * 0.5 + Mathf.Pi* lastExecutedRatio) * 0.5 + 0.5;
    double ratioTranslation = Mathf.Sin(-Mathf.Pi * 0.5 + Mathf.Pi* baseRatio) * 0.5 + 0.5;

    float ratioDiff = (float)(ratioTranslation - lastRatioTranslation);
    Vector2 movement = new Vector2(this.nextDirection.X* ratioDiff, this.nextDirection.Y* ratioDiff);
    MoveAndCollide(movement);
    this.RotationDegrees = currentRotation;
    lastExecutedRatio = baseRatio;
  }

  private static float limit(float val, float max)
  {
    return val < -max ? -max : val > max ? max : val;
  }

  private static float GetRotation(MoveDirection direction)
  {
    if(direction == MoveDirection.Up)
    {
      return 0;
    }

    if(direction == MoveDirection.Left)
    {
      return 270;
    }

    if(direction == MoveDirection.Right)
    {
      return 90;
    }

    if(direction == MoveDirection.Down)
    {
      return 180;
    }

    return 0;
  }
}