using Godot;
using System;

public class Laser : Weapon
{
  private BulletEventManager _bulletEventManager;
  private Position3D _bulletEntryPoint;
  private RayCast _rayCast;
  private ImmediateGeometry _immediateGeometry;
  private WeaponState _state = WeaponState.Inactive;
  public WeaponState State => _state;
  public bool IsShooting => State == WeaponState.Shooting;
  public bool IsInactive => State == WeaponState.Inactive;

  private int _framesBetweenBullet = 15;
  private int _framesToNextBullet = 0;

  public override void _Ready()
  {
    _bulletEventManager = GetNode<BulletEventManager>("/root/BulletEventManager");
    _bulletEntryPoint = GetNode<Position3D>("BulletEntryPoint");
    _rayCast = GetNode<RayCast>("RayCast");
    _immediateGeometry = GetNode<ImmediateGeometry>("ImmediateGeometry");
  }

  public override void _PhysicsProcess(float delta)
  {
    // _immediateGeometry.Clear();
    _rayCast.CastTo = new Vector3(_rayCast.Translation.x * 150, _rayCast.Translation.y, _rayCast.Translation.z * 150);
    // if (_rayCast.IsColliding())
    // {
    //   _immediateGeometry.Begin(Mesh.PrimitiveType.LineStrip);
    //   _immediateGeometry.AddVertex(ToLocal(_rayCast.GlobalTransform.origin));
    //   _immediateGeometry.AddVertex(ToLocal(_rayCast.GetCollisionPoint()));
    //   _immediateGeometry.End();
    // }

    if (_framesToNextBullet == 0)
    {
      if (IsShooting)
      {
        Shoot();
        _framesToNextBullet = _framesBetweenBullet;
      }
    }
    else
    {
      _framesToNextBullet--;
    }
  }

  private void Shoot()
  {
    if (_rayCast.IsColliding())
    {
      _rayCast.GetCollider
    }
    _rayCast.GetCollider();
  }

  public override void StartShooting()
  {
    if (IsShooting) { return; }
    _state = WeaponState.Shooting;
  }
  public override void StopShooting()
  {
    if (IsInactive) { return; }
    _state = WeaponState.Inactive;
  }
}
