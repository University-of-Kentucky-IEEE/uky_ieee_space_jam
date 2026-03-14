using Godot;
using System;
using UKYIEEESpaceJam;

public partial class Jar : Item
{
	public override void Use()
	{
		GD.Print("Used me jar!");
	}
}
