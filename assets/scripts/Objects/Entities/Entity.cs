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
    
    protected bool _dead;
    protected bool _justDied = false;

    public override void _PhysicsProcess(double delta)
    {
        if (_dead && _justDied) _justDied = false;
        if (!_dead && _health == 0)
        {
            _dead = true;
            _justDied = true;
        }

        if (_dead && _health > 0)
        {
            _dead = false;
            _justDied = false;
        }
    }

}