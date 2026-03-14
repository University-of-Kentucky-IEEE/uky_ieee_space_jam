using Godot;
using System;
using UKYIEEESpaceJam;

public partial class Jar : Item
{
	public enum Type
	{
		Grape,
		Apricot,
		Blackberry,
	}
	
	[Export]
	public Type JamType;

	public override void _Ready()
	{
		base._Ready();
		switch (JamType)
		{
			case Type.Grape:
				Modulate = Colors.Violet;
				break;
			case Type.Apricot:
				Modulate = Colors.Orange;
				break;
			case Type.Blackberry:
				Modulate = Colors.DarkSlateBlue;
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	public override void _Process(double delta)
	{
		GlobalRotation = 0;
	}
	
	public override void Use()
	{
		GD.Print("Used me jar!");
	}
}
