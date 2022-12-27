using Godot;
using System;

public class Zombie : KinematicBody
{
  private float _speed = 1f;
  private float _healthPoints = 50f;
  private Player _player;
  private MeshInstance _meshInstance;

  public override void _Ready()
  {
    // _player = GetNode<Player>("/root/Main/Player");
    _meshInstance = GetNode<MeshInstance>("MeshInstance");
  }

  public override void _PhysicsProcess(float delta)
  {
    if (IsQueuedForDeletion()) { return; }

    if (OS.IsDebugBuild() && Input.IsKeyPressed((int)KeyList.E))
    {
      MoveAndSlide(GlobalTranslation.DirectionTo(_player?.GlobalTranslation ?? Vector3.Zero).Normalized() * _speed * 10);
    }
    else
    {
      MoveAndSlide(GlobalTranslation.DirectionTo(_player?.GlobalTranslation ?? Vector3.Zero).Normalized() * _speed);
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

  private void _on_KinematicBody_mouse_entered() {
    _meshInstance.Hide();
  }

  private void _on_KinematicBody_mouse_exited() {
    _meshInstance.Show();
  }

  private void _on_KinematicBody_input_event(Camera camera, InputEvent @event, Vector3 position, Vector3 normal, int shape_idx)
  {
    GD.Print("DRag");
    // if (/*camera is FreeCamera &&*/ @event is InputEventMouseMotion eventMouseMotion)
    // {
    //   _cursorEventManager.NotifyPositionChanged(
    //     new CursorPositionChangedEvent
    //     {
    //       Position = position,
    //       Normal = normal,
    //       ShapeIdx = shape_idx
    //     }
    //   );
    // }
  }
}
