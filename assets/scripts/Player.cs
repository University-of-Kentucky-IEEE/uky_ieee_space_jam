using Godot;

namespace UKYIEEESpaceJam;

public partial class Player : CharacterBody2D
{
	[ExportGroup("Walking Parameters")]
	[Export]
	public float MovementFriction = 10;
	[Export]
	public float Acceleration = 100;

	
	[ExportGroup("Dashing Parameters")]
	[Export]
	public float DashSpeed = 4000;
	[Export]
	public double DashTime = 0.07;
	[Export]
	public double DashCooldown = 0.2;
	
	private static Vector2 GetInputDirection()
	{
		return Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
	}
	
	[ExportGroup("Other")]
	[Export]
	public bool IsRpg = true;
	[Export]
	public RayCast2D GroundRayCast;

	private AnimatedSprite2D _animation;
	private Vector2 _inputMovement;
	private bool _dashing;
	private Vector2 _dashDirection;
	private double _dashTimeout;
	private double _dashCooldown;

	public override void _Ready()
	{
		base._Ready();
		
		_animation = (AnimatedSprite2D)FindChild("AnimatedSprite2D");
		
		GD.Print("hello from player!");
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (_dashing)
		{
			ProcessDash(delta);
		}
		else
		{
			ProcessNormal(delta);
		}

		SetAnimation();

		MoveAndSlide();
	}

	public void ProcessNormal(double delta)
	{
		Vector2 v = GetInputDirection();
		Vector2 damp = -v;
		
		// there will be a terminal velocity tuned by the movement parameters of the character
		Vector2 currVelocity = GetVelocity();

		currVelocity += v * (float)delta * Acceleration * 125;
		currVelocity -= currVelocity / MovementFriction;
		
		SetVelocity(currVelocity);

		if (_dashCooldown > 0)
		{
			_dashCooldown -= delta;
		}
		
		if (Input.IsActionJustPressed("Dash") && _dashCooldown <= 0)
		{
			_dashing = true;
			_dashDirection = v;
			_dashTimeout = DashTime;
		}
	}

	public void ProcessDash(double delta)
	{
		SetVelocity(_dashDirection * DashSpeed);
		_dashTimeout -= delta;
		if (_dashTimeout < 0)
		{
			_dashing = false;
			_dashCooldown = DashCooldown;
		}
	}

	public void SetAnimation()
	{
		_animation.Animation = "Walking";
		if (!_dashing)
		{
			_animation.Modulate = Colors.White;
		}
		else
		{
			_animation.Modulate = Colors.IndianRed;
		}
		
		if (GetInputDirection() != Vector2.Zero)
		{
			_animation.Play();
		}
		else
		{
			_animation.Stop();
		}
	}
}
