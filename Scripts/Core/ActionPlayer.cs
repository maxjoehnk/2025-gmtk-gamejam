using System.Linq;
using gmtkgamejam.Core;
using gmtkgamejam.Scripts.Core;
using Godot;
using Godot.Collections;

namespace gmtkgamejam.Scenes;

public partial class ActionPlayer : Node
{
  public static ActionPlayer Instance { get; private set; }
  
  private double executedWaitTime;

  [Signal]
  public delegate void FinishedEventHandler();

  [Signal]
  public delegate void TickedEventHandler(int tick);

  private Timer Timer => this.GetChild<Timer>(0);

  [Export]
  public double PlaybackSpeed
  {
    get => 1 / this.Timer.WaitTime;
    set
    {
      this.Timer.WaitTime = 1 / value;
      GD.Print($"Set TickDuration to {this.Timer.WaitTime}s");
    }
  }

  public bool Preview { get; set; }

  private Array<Action> Actions { get; set; }

  private int ActionIndex { get; set; }

  public int CurrentTick { get; private set; }

  private int ActionTicksRemaining { get; set; }

  public double TickDuration => this.Timer.WaitTime;

  private Action ? CurrentAction => this.Actions.ElementAtOrDefault(this.ActionIndex);

  public override void _Ready()
  {
    Instance = this;
    this.AddChild(new Timer());
    this.Timer.Autostart = false;
    this.Timer.OneShot = true;
    this.Timer.WaitTime = 1;
    this.Timer.Timeout += this.Tick;
  }

  public override void _Process(double delta)
  {
    this.executedWaitTime += delta;
  }

  public void Play(Array<Action> actions)
  {
    this.Actions = actions;
    this.ActionTicksRemaining = this.CurrentAction?.Ticks ?? 0;

    this.Tick();
  }

  public void Stop()
  {
    this.Timer.Stop();
  }

  public void Tick()
  {
    if (this.Preview)
    {
      this.CurrentTick += 1;
      this.EmitTick();
      this.Timer.Start();
      return;
    }
    
    if(this.executedWaitTime < this.TickDuration)
    {
      this.Timer.Start();
      return;
    }
    if(this.CurrentAction == null)
    {
      return;
    }

    this.CurrentTick += 1;
    this.EmitTick();
    this.CurrentAction?.Act((Player)this.GetTree().GetFirstNodeInGroup(Groups.Player));
    this.ActionTicksRemaining -= 1;
    if (this.ActionTicksRemaining <= 0)
    {
      this.NextAction();
    }

    this.Timer.Start();

    this.executedWaitTime = 0;
  }

  private void NextAction()
  {
    this.ActionIndex += 1;
    this.ActionTicksRemaining = this.CurrentAction?.Ticks ?? 0;
    if(this.ActionIndex >= this.Actions.Count)
    {
      this.EmitSignalFinished();
    }
  }

  public void Reset()
  {
    this.Stop();
    this.CurrentTick = 0;
    this.ActionIndex = 0;
    this.ActionTicksRemaining = 0;
    this.EmitTick();
  }

  private void EmitTick()
  {
    this.EmitSignalTicked(this.CurrentTick);
    this.GetTree().CallGroup(Groups.Clocked, nameof(IClocked.OnTick), this.CurrentTick);
  }
}