using Godot;

namespace UKYIEEESpaceJam;

public partial class Player : CharacterBody2D
{
	[ExportGroup("Movement Parameters")]
	[Export]
	public float MovementFriction = 10;
	[Export]
	public float Acceleration = 100;

	private static Vector2 GetInputDirection()
	{
		return Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
	}
	
	[Export]
	public bool IsRpg = true;
	
	[Export]
	public RayCast2D GroundRayCast;

	private AnimatedSprite2D _animation = null;
	private Vector2 _inputMovement;

	public override void _Ready()
	{
		base._Ready();
		
		_animation = (AnimatedSprite2D)FindChild("AnimatedSprite2D");
		
		GD.Print("hello from player!");
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		Vector2 v = GetInputDirection();
		Vector2 damp = -v;
		
		// there will be a terminal velocity tuned by the movement parameters of the character
		Vector2 currVelocity = GetVelocity();

		currVelocity += v * (float)delta * Acceleration * 125;
		currVelocity -= currVelocity / MovementFriction;
		
		SetVelocity(currVelocity);

		if (GetInputDirection() != Vector2.Zero)
		{
			_animation.Play();
		}
		else
		{
			_animation.Stop();
		}
		
		MoveAndSlide();
	}
}
