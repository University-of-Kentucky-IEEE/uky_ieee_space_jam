using Godot;
using System;
using UKYIEEESpaceJam;
using UKYIEEESpaceJam.assets.scripts;

public partial class Enemy : Entity
{
	[Export] public EnemyDifficulty Difficulty = EnemyDifficulty.Easy;
	[Export] public float Acceleration = 100f;
	[Export] public float MovementFriction = 10f;
	[Export] public float StopRadius = 120f;
	[Export] public override double Health { get; set; }
	
	private Area2D _detectionArea;
	private CollisionShape2D _collisionShape;
	private NavigationAgent2D _navAgent;
	
	private Player _player;

	public override void _Ready()
	{
		_detectionArea = GetChild<Area2D>(0);
		_collisionShape = _detectionArea.GetChild<CollisionShape2D>(0);
		_navAgent = GetChild<NavigationAgent2D>(0);
		_navAgent.TargetDesiredDistance = StopRadius;

		// Cast the shape to CircleShape2D and change the radius
		if (_collisionShape.Shape is CircleShape2D circleShape)
		{
			switch (Difficulty)
			{
				case EnemyDifficulty.Easy:
					circleShape.Radius = 500f;
					break;
			}
		}

		_player = GetTree().CurrentScene.GetNode<Player>("Player");
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (_player != null)
		{
			float distanceToPlayer = GlobalPosition.DistanceTo(_player.GlobalPosition);
			if (distanceToPlayer <= StopRadius)
			{
				// Inside desired range: only apply friction so enemy smoothly settles.
				Velocity -= Velocity / MovementFriction;
				MoveAndSlide();
				return;
			}

			_navAgent.TargetPosition = _player.GlobalPosition;
			Vector2 nextPathPosition = _navAgent.GetNextPathPosition();
			Vector2 direction = GlobalPosition.DirectionTo(nextPathPosition);

			Vector2 currVelocity = Velocity;
			currVelocity += direction * (float)delta * Acceleration * 125f;
			currVelocity -= currVelocity / MovementFriction;
			Velocity = currVelocity;

			_navAgent.SetVelocity(Velocity);
		}
		else
		{
			// Decelerate to a stop when no player is detected
			Velocity -= Velocity / MovementFriction;
			MoveAndSlide();
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		
	}

	public void OnVelocityComputed(Vector2 safeVelocity)
	{
		Velocity = safeVelocity;
		MoveAndSlide();
	}


	public void _On_Detection_Entered(Node2D body)
	{
		if(body is Player player) _player = player;
	}

	public void _On_Detection_Exited(Node2D body)
	{
		if(body is Player) _player = null;
	}

	public void _On_Shoot_Timer()
	{
		GetNode<Hand>("Hand").HeldItem?.Use();
	}
}

public enum EnemyDifficulty
{
	Easy = 1,
	Medium = 2,
	Hard = 3
}
