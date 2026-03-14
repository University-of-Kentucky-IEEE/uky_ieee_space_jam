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
	
	private Player? _player;
	
	private Vector2 _startPosition;
	
	private Timer _respawnTimer;
	private double _stunCooldown;

	public override void _Ready()
	{
		_startPosition = GlobalPosition;
		_detectionArea = GetNode<Area2D>("Area2D");
		_collisionShape = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");
		_navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		_navAgent.TargetDesiredDistance = StopRadius;

		// Cast the shape to CircleShape2D and change the radius
		if (_collisionShape.Shape is CircleShape2D circleShape)
		{
			switch (Difficulty)
			{
				case EnemyDifficulty.Easy:
					circleShape.Radius = 500f;
					break;
				case EnemyDifficulty.Medium:
					circleShape.Radius = 1000f;
					break;
			}
		}
		
		_respawnTimer = GetNode<Timer>("RespawnTimer");
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (_justDied)
		{
			GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
			Visible = false;
			_respawnTimer.Start();
		}
		if (_dead) return;
		if (_stunCooldown > 0)
		{
			_stunCooldown -= delta;
		}
		else
		{
			Stunned = false;
		}
		
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
		}
		else
		{
			// Decelerate to a stop when no player is detected
			Velocity -= Velocity / MovementFriction;
		}
		MoveAndSlide();
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

	public override void Damage(double amount)
	{
		base.Damage(amount);
		GD.Print("Enemy health: " + Health);
		GD.Print("is dead: " + _dead);
	}

	public void Shove(Vector2 shove)
	{
		Velocity = shove;
		Stunned = true;
		_stunCooldown = StunCooldown;
	}

	[Export]
	public double StunCooldown = 0.4;
	
	public bool Stunned;

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

	public void _On_Respawn_Timer()
	{
		GlobalPosition = _startPosition;
		Visible = true;
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
		Health = MaxHealth;
	}

	public bool Dead()
	{
		return _dead;
	}
}

public enum EnemyDifficulty
{
	Easy = 1,
	Medium = 2,
	Hard = 3
}
