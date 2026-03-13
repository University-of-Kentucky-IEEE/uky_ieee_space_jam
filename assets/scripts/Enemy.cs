using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export] public EnemyDifficulty difficulty = EnemyDifficulty.Easy;
	
	public override void _Ready()
	{
		
	}

	public override void _Process(double delta)
	{
		
	}
}

public enum EnemyDifficulty
{
	Easy = 1,
	Medium = 2,
	Hard = 3
}