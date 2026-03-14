using Godot;
using System;
using UKYIEEESpaceJam;

public partial class Hand : Node2D
{
	
	private Node2D _mountPoint;
	public Item? HeldItem { get; private set; }

	public override void _Ready()
	{
		_mountPoint = GetNode<Node2D>("Mount");
	}
	public override void _Process(double delta)
	{
		Vector2 mousePosition = GetGlobalMousePosition();
		float rads = MathF.Atan2(-(mousePosition.Y - GetGlobalPosition().Y), mousePosition.X - GetGlobalPosition().X);

		Rotation = rads;
	}

	public void PickupItem(Item item)
	{
		if (HeldItem != null) return;
		item.Reparent(_mountPoint);
		HeldItem = item;
	}

	public void DropItem(Item item)
	{
		if(item == null) return;
		item.Reparent(GetTree().GetCurrentScene(), true);
		item.Rotation = 0;
	}
}
