using Godot;

namespace UKYIEEESpaceJam.assets.scripts;

public partial class Player : Entity
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

	[Export]
	public double IdleCost = 0.5;
	[Export]
	public double MoveCost = 2;
	[Export]
	public double DashCost = 10;
	
	[Export]
	public override double Health 
	{
		get => _health;
		set {
			_health = value;
			if (_healthBar != null) _healthBar.Value = value / MaxHealth * 100;
		}
	}
	
	private static Vector2 GetInputDirection()
	{
		return Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
	}
	
	[ExportGroup("Other")]
	[Export]
	public bool IsRpg = true;
	[Export]
	public RayCast2D GroundRayCast;

	private Vector2 _inputMovement;
	
	private AnimatedSprite2D _animation;
	private ProgressBar _healthBar;
	
	private bool _dashing;
	private Vector2 _dashDirection;
	private double _dashTimeout;
	private double _dashCooldown;


	public override void _Ready()
	{
		base._Ready();
		
		_animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_healthBar = GetTree().GetCurrentScene().GetNode<ProgressBar>("CanvasLayer/HUD/ProgressBar");
		
		GD.Print("hello from player!");
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (!_dead)
		{
			if (_dashing)
			{
				ProcessDash(delta);
			}
			else
			{
				ProcessNormal(delta);
			}

			if (Health <= 0)
			{
				_dead = true;
			}
		}
		else
		{
			ProcessDead(delta);
		}

		SetAnimation();

		MoveAndSlide();
	}

	private void ProcessNormal(double delta)
	{
		Vector2 v = GetInputDirection();
		
		// there will be a terminal velocity tuned by the movement parameters of the character
		Vector2 currVelocity = GetVelocity();

		currVelocity += v * (float)delta * Acceleration * 125;
		currVelocity -= currVelocity / MovementFriction;
		
		SetVelocity(currVelocity);

		if (_dashCooldown > 0)
		{
			_dashCooldown -= delta;
		}

		if (GetVelocity() != Vector2.Zero)
		{
			Health -= MoveCost * delta;
		}
		else
		{
			Health -= IdleCost * delta;
		}
		
		if (Input.IsActionJustPressed("Dash") && _dashCooldown <= 0)
		{
			_dashing = true;
			_dashDirection = v;
			_dashTimeout = DashTime;
		}

		if (Input.IsActionJustPressed("Shoot"))
		{
			GetNode<Hand>("Hand").HeldItem?.Use();
		}
	}

	private void ProcessDash(double delta)
	{
		SetVelocity(_dashDirection * DashSpeed);
		_dashTimeout -= delta;
		Health -= DashCost * delta;
		
		if (_dashTimeout < 0)
		{
			_dashing = false;
			_dashCooldown = DashCooldown;
		}
	}

	private void ProcessDead(double delta)
	{
		SetVelocity(Vector2.Zero);
	}

	private void SetAnimation()
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
