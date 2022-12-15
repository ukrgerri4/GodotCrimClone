using Godot;
using System;

public class Zombie : KinematicBody
{
  private float _speed = 7.5f;
  private float _healthPoints = 50f;
  private Player _player;

  public override void _Ready()
  {
    _player = GetNode<Player>("/root/Main/Player");
  }

  public override void _PhysicsProcess(float delta)
  {
    if (IsQueuedForDeletion()) { return; }

    MoveAndSlide(GlobalTranslation.DirectionTo(_player.GlobalTranslation).Normalized() * _speed);
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
      // HealthBarUpdate(_healthPoints);
    }
  }
}
