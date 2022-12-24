using Godot;
using System;

public class Zombie : KinematicBody
{
  private float _speed = 1f;
  private float _healthPoints = 50f;
  private Player _player;

  public override void _Ready()
  {
    _player = GetNode<Player>("/root/Main/Player");
  }

  public override void _PhysicsProcess(float delta)
  {
    if (IsQueuedForDeletion()) { return; }

    if (OS.IsDebugBuild() && Input.IsKeyPressed((int)KeyList.E))
    {
      MoveAndSlide(GlobalTranslation.DirectionTo(_player.GlobalTranslation).Normalized() * _speed * 10);
    }
    else
    {
      MoveAndSlide(GlobalTranslation.DirectionTo(_player.GlobalTranslation).Normalized() * _speed);
    }
  }

  public float HandleHit(float damage)
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
    return _healthPoints;
  }
}
