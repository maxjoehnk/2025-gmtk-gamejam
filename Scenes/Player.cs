using Godot;
using gmtkgamejam.Core;
using gmtkgamejam.Scenes;
using Vector2 = Godot.Vector2;

public partial class Player : StaticBody2D
{
  private const int GridSize = 128;

  [Export] public float RotationSpeed { get; set; } = Mathf.Pi * 2 * 180 / Mathf.Pi;

  private ActionPlayer Playback => ActionPlayer.Get(this);
  private Vector2 nextDirection = new Vector2(0, 0);
  private float nextRotation = 0;
  private float currentRotation = 0;
  private double executedTime = 0;

  public void Move(MoveDirection direction)
  {
    nextDirection = GetMovementVector(direction);
    executedTime = 0;
    nextRotation = GetRotation(direction);

    KinematicCollision2D ? collision2D = MoveAndCollide(nextDirection, testOnly: true);
    if(collision2D != null)
    {
      nextDirection = new Vector2(0, 0);
      return;
    }

    // MoveAndCollide(moveVector);
  }

  public override void _Process(double delta)
  {
    if(nextDirection == new Vector2(0, 0))
      return;

    double lastExecutedTime = executedTime;
    executedTime += delta;

    float maxRotationPerUpdate = this.RotationSpeed* (float)delta;
    GD.Print(maxRotationPerUpdate);
    GD.Print(currentRotation - nextRotation);
    GD.Print(limit(currentRotation - nextRotation, maxRotationPerUpdate));
    GD.Print("---");
    currentRotation += limit(nextRotation - currentRotation, maxRotationPerUpdate);

    double lastBaseRatio = Mathf.Min(1, lastExecutedTime / this.Playback.TickDuration);
    double baseRatio = Mathf.Min(1, this.executedTime / this.Playback.TickDuration);

    double lastRatioTranslation = Mathf.Sin(-Mathf.Pi * 0.5 + Mathf.Pi* lastBaseRatio) * 0.5 + 0.5;
    double ratioTranslation = Mathf.Sin(-Mathf.Pi * 0.5 + Mathf.Pi* baseRatio) * 0.5 + 0.5;

    float ratioDiff = (float)(ratioTranslation - lastRatioTranslation);
    Vector2 movement = new Vector2(this.nextDirection.X* ratioDiff, this.nextDirection.Y* ratioDiff);
    MoveAndCollide(movement);
    this.RotationDegrees = currentRotation;
  }

  private static float limit(float val, float max)
  {
    return val < -max ? -max : val > max ? max : val;
  }

  private static Vector2 GetMovementVector(MoveDirection direction)
  {
    if(direction == MoveDirection.Up)
    {
      return new Vector2(0, -GridSize);
    }

    if(direction == MoveDirection.Left)
    {
      return new Vector2(-GridSize, 0);
    }

    if(direction == MoveDirection.Right)
    {
      return new Vector2(GridSize, 0);
    }

    if(direction == MoveDirection.Down)
    {
      return new Vector2(0, GridSize);
    }

    return Vector2.Zero;
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