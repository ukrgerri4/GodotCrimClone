using System;
using Godot;

public class Bullet : MeshInstance
{
  private BulletEventManager _bulletEventManager;
  private PhysicsDirectSpaceState _directSpaceState;

  private float _speed = BulletDefaultOptions.DefaultSpeed;
  private float _deviationRadians = BulletDefaultOptions.DeviationRadians;
  private float _existingTimeSec = BulletDefaultOptions.MaxExistingTimeSec;
  private Vector3 _direction;

  public override void _Ready()
  {
    _bulletEventManager = GetNode<BulletEventManager>("/root/BulletEventManager");
    _directSpaceState = GetWorld().DirectSpaceState;
    SetAsToplevel(true);
  }

  public override void _PhysicsProcess(float delta)
  {
    _existingTimeSec -= delta;
    if (_existingTimeSec <= 0)
    {
      Disable();
    }

    Move(delta);

    // var result = CheckCollision();
    var result = _directSpaceState.IntersectPoint(GlobalTranslation, maxResults: 1, collisionLayer: 2);
    if (result.Count > 0)
    {
      Disable();
      if (result[0] is Godot.Collections.Dictionary dict && dict.Contains("collider") && dict["collider"] is SphereObject so) {
        so.QueueFree();
      }
    }
  }

  private Godot.Collections.Array CheckCollision()
  {
    var query = new PhysicsShapeQueryParameters();
    query.SetShape(new Resource());
    return GetWorld().DirectSpaceState.IntersectShape(query, 1);
  }

  private void Move(float delta)
  {
    Translation += _direction * delta * _speed;
  }

  public void Disable()
  {
    SetPhysicsProcess(false);
    SetProcess(false);
    Hide();
    _bulletEventManager.FreeBullets.Enqueue(this);
  }

  public void Enable(Vector3 entryPoint, Vector3 direction)
  {
    Translation = entryPoint;

    _direction = direction.Rotated(
      Vector3.Up,
      (float)GD.RandRange(-1 * _deviationRadians, _deviationRadians)
    );

    _existingTimeSec = BulletDefaultOptions.MaxExistingTimeSec;

    SetPhysicsProcess(true);
    SetProcess(true);
    Show();
  }
}
