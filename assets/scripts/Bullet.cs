using Godot;
using System;

public partial class Bullet : CharacterBody2D
{
	private double _timer = 3;
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		_timer -= delta;

		if (_timer <= 0)
		{
			Die();
		}
		
		MoveAndSlide();
	}

	private void Die()
	{
		QueueFree();
	}
}
