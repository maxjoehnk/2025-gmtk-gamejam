using System.Linq;
using gmtkgamejam.Core;
using Godot;
using Godot.Collections;

namespace gmtkgamejam.Scenes;

public partial class ActionPlayer : Node
{
  public static ActionPlayer Get(Node node)
  {
    return (ActionPlayer)node.GetTree().GetFirstNodeInGroup("ActionPlayer");
  }

  [Signal]
  public delegate void FinishedEventHandler();

  [Signal]
  public delegate void TickedEventHandler(int tick);

  private Timer ? Timer => this.GetNodeOrNull<Timer>("Timer");

  [Export]
  public double PlaybackSpeed
  {
    get => 1 / (this.Timer?.WaitTime ?? 1);
    set
    {
      if(this.Timer == null)
      {
        return;
      }

      this.Timer.WaitTime = 1 / value;
      GD.Print($"Set TickDuration to {this.Timer.WaitTime}s");
    }
  }

  private Array<Action> Actions { get; set; }

  private int ActionIndex { get; set; }

  public int CurrentTick { get; private set; }

  private int ActionTicksRemaining { get; set; }

  public double TickDuration => this.Timer?.WaitTime ?? 1;

  private Action ? CurrentAction => Actions.ElementAtOrDefault(ActionIndex);

  private double executedWaitTime = 0;

  public void Play(Array<Action> actions)
  {
    Actions = actions;
    ActionTicksRemaining = CurrentAction?.Ticks ?? 0;

    Tick();
  }

  public void Stop()
  {
    this.Timer?.Stop();
  }

  public void Tick()
  {
    if(executedWaitTime < TickDuration)
    {
      this.Timer?.Start();
      return;
    }
    if(CurrentAction == null)
    {
      return;
    }

    CurrentTick += 1;
    EmitSignalTicked(CurrentTick);
    CurrentAction?.Act(GetParent<Game>().Player);
    ActionTicksRemaining -= 1;
    if(ActionTicksRemaining <= 0)
    {
      NextAction();
    }

    this.Timer?.Start();

    executedWaitTime = 0;
  }

  private void NextAction()
  {
    this.ActionIndex += 1;
    this.ActionTicksRemaining = CurrentAction?.Ticks ?? 0;
    if(ActionIndex >= Actions.Count)
    {
      EmitSignalFinished();
    }
  }

  public void Reset()
  {
    this.Stop();
    CurrentTick = 0;
    ActionIndex = 0;
    ActionTicksRemaining = 0;
    EmitSignalTicked(CurrentTick);
  }

  public override void _Process(double delta)
  {
    executedWaitTime += delta;
  }
}