using Godot;

namespace UKYIEEESpaceJam.assets.scripts;

public partial class Entity : CharacterBody2D
{
    [ExportGroup("Status Parameters")]
    [Export]
    public double MaxHealth = 100;

    protected double _health = 100;

    [Export]
    public virtual double Health { get; set; }
}