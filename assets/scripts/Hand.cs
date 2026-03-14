using Godot;
using System;
using UKYIEEESpaceJam;

public partial class Hand : Node2D
{
	
	private Node2D _mountPoint;
	private Node2D _heldItem;

	public override void _Ready()
	{
		_mountPoint = GetNode<Node2D>("Mount");
	}
	public override void _Process(double delta)
	{
		Vector2 localMousePosition = GetLocalMousePosition();
		float rads = MathF.Atan2(-localMousePosition.Y, localMousePosition.X);

		Rotation = rads;
	}

	public void PickupItem(Item item)
	{
		item.Reparent(_mountPoint);
	}

	public void DropItem(Item item)
	{
		item.Reparent(GetTree().GetCurrentScene(), true);
		item.Rotation = 0;
	}
}
