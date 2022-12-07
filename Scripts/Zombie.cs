using Godot;
using System;

public class Zombie : KinematicBody
{
  private HealthBar3D _healthBar;

  private float _speed = 7f;
  private float _healthPoints = 100f;

  public override void _Ready()
  {
    _healthBar = GD.Load<PackedScene>("res://Scenes/Components/HealthBar3D.tscn").Instance<HealthBar3D>();
    _healthBar.MaxValue = _healthPoints;
    _healthBar.Translation = new Vector3(0, 1.25f, 0);
    AddChild(_healthBar);
  }

  public override void _PhysicsProcess(float delta)
  {
    if (IsQueuedForDeletion()) { return; }

    MoveAndSlide(-1 * Translation * _speed * delta);
  }

  public void HandleHit(float damage)
  {
    _healthPoints -= damage;
    if (_healthPoints <= 0)
    {
      QueueFree();
    }
    else
    {
      _healthBar.Update(_healthPoints);
    }
  }
}
