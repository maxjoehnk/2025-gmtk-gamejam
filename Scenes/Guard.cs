using Godot;
using System.Linq;
using Godot.Collections;

public partial class Guard : Node2D
{
	[Export] public float Speed { get; set; } = 128;

	private CharacterBody2D Character => GetNode<CharacterBody2D>("CharacterBody2D");
	private NavigationAgent2D NavigationAgent => GetNode<NavigationAgent2D>("CharacterBody2D/NavigationAgent2D");

	private Array<Waypoint> Path => new(GetChildren().OfType<Waypoint>());

	private int CurrentPathIndex { get; set; }

	private Vector2? CurrentWaypoint => Path.ElementAtOrDefault(CurrentPathIndex)?.GlobalPosition;
	
	private float _movementDelta;

	public override void _Ready()
	{
		NavigationAgent.VelocityComputed += OnVelocityComputed;

		MoveToWaypoint();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (NavigationServer2D.MapGetIterationId(NavigationAgent.GetNavigationMap()) == 0)
		{
			return;
		}
		if (NavigationAgent.IsNavigationFinished())
		{
			return;
		}
		
		_movementDelta = Speed * (float)delta;
		Vector2 nextPathPosition = NavigationAgent.GetNextPathPosition();
		Vector2 newVelocity = Character.GlobalPosition.DirectionTo(nextPathPosition) * _movementDelta;
		if (NavigationAgent.AvoidanceEnabled)
		{
			NavigationAgent.Velocity = newVelocity;
		}
		else
		{
			OnVelocityComputed(newVelocity);
		}
	}

	public void ReachedWaypoint()
	{
		CurrentPathIndex += 1;
		if (CurrentPathIndex >= Path.Count)
		{
			CurrentPathIndex = 0;
		}

		MoveToWaypoint();
	}

	private void MoveToWaypoint()
	{
		Vector2? currentWaypoint = CurrentWaypoint;
		if (currentWaypoint == null)
		{
			return;
		}

		GD.Print("Moving to waypoint at: ", currentWaypoint.Value);
		NavigationAgent.TargetPosition = currentWaypoint.Value;
	}

	private void OnVelocityComputed(Vector2 safeVelocity)
	{
		Character.GlobalPosition = Character.GlobalPosition.MoveToward(Character.GlobalPosition + safeVelocity, _movementDelta);
	}
}