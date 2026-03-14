using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using UKYIEEESpaceJam;

public partial class Hand : Node2D
{
	
	private Node2D _mountPoint;
	public Item? HeldItem { get; private set; }
	
	private Item[] _reachableItems;

	public override void _Ready()
	{
		_mountPoint = GetNode<Node2D>("Mount");
		if (_mountPoint.GetChildren().Count > 0)
		{
			HeldItem = _mountPoint.GetChild<Item>(0);
		}
	}
	public override void _Process(double delta)
	{
		Vector2 mousePosition = GetGlobalMousePosition();
		float rads = MathF.Atan2((mousePosition.Y - GetGlobalPosition().Y), mousePosition.X - GetGlobalPosition().X);

		Rotation = rads;

		_reachableItems = GetNode<Area2D>("Area2D").GetOverlappingAreas().Select(x => x.GetParent<Item>()).ToArray();
		
	}

	public void PickupItem()
	{
		if (HeldItem != null) return;
		
		if (_reachableItems[0].GetParent() == null)
			_mountPoint.AddChild(_reachableItems[0]);
		else
			_reachableItems[0].Reparent(_mountPoint);
		
		HeldItem = _reachableItems[0];
	}

	public void DropItem()
	{
		if (HeldItem == null) return;
		HeldItem.Reparent(GetTree().GetCurrentScene(), true);
		HeldItem.Rotation = 0;
		HeldItem = null;
	}
}
