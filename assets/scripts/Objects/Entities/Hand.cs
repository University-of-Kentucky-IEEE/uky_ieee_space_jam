using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using UKYIEEESpaceJam;

public partial class Hand : Node2D
{
	
	private Node2D _mountPoint;
	public Item? HeldItem { get; private set; }
	
	public bool IsHoldingItem => HeldItem != null;

	private List<Item> _reachableItems;

	public override void _Ready()
	{
		_mountPoint = GetNode<Node2D>("Mount");
		if (_mountPoint.GetChildren().Count > 0)
		{
			HeldItem = _mountPoint.GetChild<Item>(0);
		}

		_reachableItems = new List<Item>();
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 mousePosition = GetGlobalMousePosition();
		float rads = MathF.Atan2((mousePosition.Y - GetGlobalPosition().Y), mousePosition.X - GetGlobalPosition().X);

		Rotation = rads;
	}

	public void PickupItem()
	{
		if (HeldItem != null) return;

		if (_reachableItems[0].GetParent() == null)
		{
			_mountPoint.AddChild(_reachableItems[0]);
			_reachableItems[0].GlobalPosition = _mountPoint.GlobalPosition;
			_reachableItems[0].GlobalRotation = _mountPoint.GlobalRotation;
		}
		else
		{
			_reachableItems[0].Reparent(_mountPoint, false);
			_reachableItems[0].GlobalPosition = _mountPoint.GlobalPosition;
			_reachableItems[0].GlobalRotation = _mountPoint.GlobalRotation;
		}
		
		HeldItem = _reachableItems[0];
	}

	public void DropItem()
	{
		if (HeldItem == null) return;
		HeldItem.Reparent(GetTree().GetCurrentScene(), true);
		HeldItem.Rotation = 0;
		HeldItem = null;
	}

	public void GiveItem(Item item)
	{
		if (IsHoldingItem) DropItem();
		HeldItem = item;
		_mountPoint.AddChild(item);
		item.GlobalPosition = _mountPoint.GlobalPosition;
		item.GlobalRotation = _mountPoint.GlobalRotation;
	}

	private void OnHandEnteredReach(Area2D area)
	{
		GD.Print(area);
		if (area.GetParent() is Item i)
		{
			_reachableItems.Insert(_reachableItems.Count, i);
			GD.Print(_reachableItems.Count);
		}
	}
	
	private void OnHandExitedReach(Area2D area)
	{
		_reachableItems.RemoveAll(i => i == area.GetParent());
	}
}
