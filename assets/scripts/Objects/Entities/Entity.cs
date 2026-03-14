using Godot;

namespace UKYIEEESpaceJam.assets.scripts;

public abstract partial class Entity : CharacterBody2D
{
    [ExportGroup("Status Parameters")]
    [Export]
    public double MaxHealth = 100;

    protected double _health = 100;

    [Export]
    public abstract double Health { get; set; }
}