using Godot;
using gmtkgamejam.Core;
using gmtkgamejam.Scenes;
using gmtkgamejam.Scripts.Core;
using Vector2 = Godot.Vector2;
using static Godot.TextServer;

public partial class Player : CharacterBody2D
{
  [Export] public float RotationSpeed { get; set; } = 360;

  private RayCast2D RayCast => GetNode<RayCast2D>("RayCast2D");
  
  private ActionPlayer Playback => ActionPlayer.Get(this);
  private Vector2 nextDirection = new(0, 0);
  private float nextRotation;
  private float currentRotation;
  private double executedTime;
  private double lastExecutedRatio;

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

  public IInteractable? GetInteractableElement()
  {
    GodotObject collision = this.RayCast.GetCollider();
    if (collision is IInteractable interactable)
    {
      return interactable;
    }

    return null;
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