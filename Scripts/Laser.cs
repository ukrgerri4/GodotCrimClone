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
    _immediateGeometry.SetAsToplevel(true);
  }

  public override void _PhysicsProcess(float delta)
  {
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
    // _immediateGeometry.Clear();
    // _immediateGeometry.Begin(Mesh.PrimitiveType.LineStrip);
    // _immediateGeometry.AddVertex(ToLocal(_bulletEntryPoint.GlobalTranslation));
    // var secondPoint = ((-1 * new Vector3(GlobalTranslation.x, _bulletEntryPoint.GlobalTranslation.y, GlobalTranslation.z) + _bulletEntryPoint.GlobalTranslation)).Normalized() * 150;
    // GD.Print(_bulletEntryPoint.GlobalTranslation, "   ", secondPoint);
    // _immediateGeometry.AddVertex(ToLocal(secondPoint));
    // _immediateGeometry.End();

    _immediateGeometry.Begin(Mesh.PrimitiveType.LineStrip);
    _immediateGeometry.AddVertex(_immediateGeometry.ToLocal(_bulletEntryPoint.GlobalTranslation));
    var secondPoint = ((-1 * new Vector3(GlobalTranslation.x, _bulletEntryPoint.GlobalTranslation.y, GlobalTranslation.z) + _bulletEntryPoint.GlobalTranslation)).Normalized() * 150;
    GD.Print(_bulletEntryPoint.GlobalTranslation, "   ", secondPoint);
    _immediateGeometry.AddVertex(_immediateGeometry.ToLocal(secondPoint));
    _immediateGeometry.End();
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
