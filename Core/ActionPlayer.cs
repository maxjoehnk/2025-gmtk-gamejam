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

	private Timer Timer => GetNode<Timer>("Timer");

	private Array<Action> Actions { get; set; }

	private int ActionIndex { get; set; }

	private int CurrentTick { get; set; }

	private int ActionTicksRemaining { get; set; }

	public double TickDuration => this.Timer.WaitTime;


	private Action? CurrentAction => Actions.ElementAtOrDefault(ActionIndex);

	public void Play(Array<Action> actions)
	{
		Actions = actions;
		ActionTicksRemaining = CurrentAction?.Ticks ?? 0;

		Tick();
	}

	public void Stop()
	{
		this.Timer.Stop();
	}

	public void Tick()
	{
		if (CurrentAction == null)
		{
			return;
		}
		CurrentTick += 1;
		EmitSignalTicked(CurrentTick);
		CurrentAction?.Act(GetParent<Game>().Player);
		ActionTicksRemaining -= 1;
		if (ActionTicksRemaining <= 0)
		{
			NextAction();
		}

		this.Timer.Start();
	}

	private void NextAction()
	{
		this.ActionIndex += 1;
		this.ActionTicksRemaining = CurrentAction?.Ticks ?? 0;
		if (ActionIndex >= Actions.Count)
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
}