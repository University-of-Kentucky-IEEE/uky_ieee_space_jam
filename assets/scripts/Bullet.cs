using Godot;
using System;
using UKYIEEESpaceJam.assets.scripts;

public partial class Bullet : CharacterBody2D
{
	private double _timer = 3;
	private RayCast2D _rayCast;

	public override void _Ready()
	{
		_rayCast = GetNode<RayCast2D>("RayCast2D");
	}
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		_timer -= delta;

		if (_timer <= 0)
		{
			Die();
		}
		
		MoveAndSlide();

		if (_rayCast.IsColliding() && _rayCast.GetCollider() is Entity entity)
		{
			if (entity is Player) entity.Damage(10);
			else entity.Damage(25);
			Die();
		}
	}

	private void Die()
	{
		QueueFree();
	}
}
