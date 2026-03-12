using Godot;

namespace UKYIEEESpaceJam;

public partial class Player : CharacterBody2D
{
	private const int MaxSpeed = 100;

	private AnimatedSprite2D Animation = null;

	public override void _Ready()
	{
		base._Ready();
		
		Animation = (AnimatedSprite2D)FindChild("AnimatedSprite2D");
		
		GD.Print("hello!");
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		SetVelocity(Vector2.Zero);
		
		// Make a 2D RPG-style mover
		if (Input.IsActionPressed("MoveLeft"))
			SetVelocity(GetVelocity() - new Vector2(MaxSpeed, 0));
		if (Input.IsActionPressed("MoveRight"))
			SetVelocity(GetVelocity() + new Vector2(MaxSpeed, 0));
		if (Input.IsActionPressed("MoveUp"))
			SetVelocity(GetVelocity() - new Vector2(0, MaxSpeed));
		if (Input.IsActionPressed("MoveDown"))
			SetVelocity(GetVelocity() + new Vector2(0, MaxSpeed));

		if (GetVelocity() != Vector2.Zero)
		{
			Animation.Play();
		}
		else
		{
			Animation.Stop();
		}
		
		MoveAndSlide();
	}
}
