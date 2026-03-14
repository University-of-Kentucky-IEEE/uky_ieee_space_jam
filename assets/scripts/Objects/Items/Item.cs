using Godot;
using UKYIEEESpaceJam.assets.scripts;

namespace UKYIEEESpaceJam;

public partial class Item : Node2D
{
	public Entity? Holder
	{
		get
		{
			Hand? hand = GetParentOrNull<Node2D>()?.GetParentOrNull<Hand>();
			if (hand != null) return hand.GetParent<Entity>();
			return null;
		}
	}

	public virtual void Use() {}
}
