using Godot;
using System;
using UKYIEEESpaceJam;

public partial class Gun : Item
{
	[Export]
	public float BulletSpeed = 3000;

	private PackedScene _bulletScene = GD.Load<PackedScene>("res://assets/objects/Bullet.tscn");
	
	public override void Use()
	{
		float rotation = GlobalRotation;

		Bullet b = _bulletScene.Instantiate<Bullet>();
		b.Velocity = Vector2.Right.Rotated(rotation) * BulletSpeed;
		
		GetTree().GetCurrentScene().AddChild(b);
		b.GlobalPosition = GlobalPosition;
		GD.Print("YIPPEEE");
	}
}
