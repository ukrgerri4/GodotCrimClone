using Godot;
using System;
using System.Threading.Tasks;
using System.Timers;

public class AutoShotgun : Weapon
{
  private BulletEventManager _bulletEventManager;
  private Position3D _bulletEntryPoint;
  private CursorEventManager _cursorEventManager;
  
  private WeaponState _state = WeaponState.Inactive;
  public WeaponState State => _state;
  public bool IsShooting => State == WeaponState.Shooting;
  public bool IsInactive => State == WeaponState.Inactive;

  public int MaxAmmoCapacity { get; set; } = 300;
  public int AmmoCapacity { get; set; } = 30;
  public int MagazineCapacity { get; set; } = 30;
  public int EffectiveRange { get; set; } = 30;

  private int _framesBetweenBullet = 5;
  private int _framesToNextBullet = 0;

  public override void _Ready()
  {
    _bulletEventManager = GetNode<BulletEventManager>("/root/BulletEventManager");
    _bulletEntryPoint = GetNode<Position3D>("BulletEntryPoint");
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
    var bullet = _bulletEventManager.FreeBullets.Dequeue();

    if (!bullet.IsInsideTree())
    {
      AddChild(bullet);
    }

    bullet.Enable(
      _bulletEntryPoint.GlobalTranslation,
      -1 * new Vector3(GlobalTranslation.x, _bulletEntryPoint.GlobalTranslation.y, GlobalTranslation.z) + _bulletEntryPoint.GlobalTranslation
    );
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
