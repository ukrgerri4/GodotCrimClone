using System;
using Godot;

public class MachineGunBullet : MeshInstance
{
  private BulletEventManager _bulletEventManager;
  private PhysicsDirectSpaceState _directSpaceState;

  private Vector3 _direction;
  private float _speed = BulletDefaultOptions.DefaultSpeed;
  private float _deviationRadians = BulletDefaultOptions.DeviationRadians;
  private float _existingTimeSec = BulletDefaultOptions.MaxExistingTimeSec;
  private float _damage = BulletDefaultOptions.BaseDamage;

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

    var result = _directSpaceState.IntersectPoint(GlobalTranslation, maxResults: 1, collisionLayer: 4);
    if (result.Count > 0)
    {
      Disable();
      if (result[0] is Godot.Collections.Dictionary dict && dict.Contains("collider") && dict["collider"] is Zombie zombie) {
        zombie.HandleHit(_damage);
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
    Translation += _direction * _speed * delta;
  }

  public void Disable()
  {
    SetPhysicsProcess(false);
    // SetProcess(false);
    // SetProcessInput(false);
    Hide();
    _bulletEventManager.EnqueueBullet(this);
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
    // SetProcess(true);
    // SetProcessInput(true);
    Show();
  }
}
