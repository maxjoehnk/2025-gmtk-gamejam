using System;
using gmtkgamejam.Scenes;
using Godot;

public partial class Guard : PathFollow2D
{
  private ActionPlayer Playback => ActionPlayer.Get(this);

  [Export] public float Speed { get; set; } = 128;

  [Export] public double InitialPoint { get; set; } = 4;

  [Signal]
  public delegate void CatchedPlayerEventHandler();

  private CharacterBody2D Character => GetNode<CharacterBody2D>("CharacterBody2D");

  private int targetPoint = 0;
  private int lastPoint = 0;
  private double currentSubTime = 0;

  public override void _Ready()
  {
    Playback.Ticked += OnTick;

    this.targetPoint = (int)this.InitialPoint;
    this.lastPoint = (int)this.InitialPoint;
    this.currentSubTime = 0;
  }

  private void OnTick(int tick)
  {
    this.targetPoint = (int)this.InitialPoint + tick; // replace with waittime
    if(tick == 0)
    {
      this.targetPoint = (int)this.InitialPoint;
      this.lastPoint = (int)this.InitialPoint;
      this.currentSubTime = 0;
      this.Progress = 0;
    }
  }

  public void OnHitPlayer(Node2D player)
  {
    GD.Print($"Hit player {player} ({player.Name})");
    this.EmitSignalCatchedPlayer();
  }

  public override void _Process(double delta)
  {
    this.currentSubTime += delta;
    if(currentSubTime > this.Playback.TickDuration)
    {
      this.lastPoint = Math.Min(this.lastPoint + 1, this.targetPoint);
      this.currentSubTime -= this.Playback.TickDuration;
    }
    if(this.lastPoint == this.targetPoint)
      this.currentSubTime = 0;
    double ratio = Mathf.Sin(-Mathf.Pi * 0.5 + Mathf.Pi* this.currentSubTime / this.Playback.TickDuration) * 0.5 + 0.5;
    double interpolatedPoint = this.lastPoint * (1 - ratio) + Math.Min(this.lastPoint + 1, this.targetPoint) * ratio;
    this.Progress = this.Speed* (float)interpolatedPoint;
  }
}