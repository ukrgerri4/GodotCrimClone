using Godot;
using System;
using System.Threading.Tasks;
using System.Timers;

public class AutoShotgun : Weapon
{
  private BulletEventManager _bulletEventManager;
  private Position3D _bulletEntryPoint;
  private System.Timers.Timer _shootingTimer;

  public int MaxAmmoCapacity { get; set; } = 300;
  public int AmmoCapacity { get; set; } = 30;
  public int MagazineCapacity { get; set; } = 30;
  public int EffectiveRange { get; set; } = 30;

  public int _rateOfFirePerSecond;
  public int RateOfFirePerSecond
  {
    get => _rateOfFirePerSecond;
    set
    {
      _rateOfFirePerSecond = value;
      _minShootingIntervalMilliseconds = (int)(1000 / _rateOfFirePerSecond);
    }
  }

  private int _minShootingIntervalMilliseconds;
  private WeaponState _state = WeaponState.Inactive;
  public WeaponState State => _state;
  public bool IsShooting => State == WeaponState.Shooting;
  public bool IsInactive => State == WeaponState.Inactive;

  public override void _Ready()
  {
    _bulletEventManager = GetNode<BulletEventManager>("/root/BulletEventManager");
    _bulletEntryPoint = GetNode<Position3D>("BulletEntryPoint");

    RateOfFirePerSecond = 15;

    _shootingTimer = new System.Timers.Timer();
    _shootingTimer.Interval = _minShootingIntervalMilliseconds;
    _shootingTimer.Elapsed += OnTimeout;
    _shootingTimer.AutoReset = true;
  }

  private void OnTimeout(object sender, ElapsedEventArgs e)
  {
    if (IsShooting)
    {
      Shoot();
    }
  }


  private void Shoot()
  {
    _bulletEventManager.AddBullet(
      new ShotEvent
      {
        EntryPoint = _bulletEntryPoint.GlobalTranslation,
        Direction = -1 * new Vector3(GlobalTranslation.x, _bulletEntryPoint.GlobalTranslation.y, GlobalTranslation.z) + _bulletEntryPoint.GlobalTranslation
      }
    );
  }

  public override void StartShooting()
  {
    // GD.Print("start");
    if (IsShooting) { return; }

    _state = WeaponState.Shooting;
    Shoot();
    _shootingTimer.Start();
  }
  public override void StopShooting()
  {
    // GD.Print("stop");
    if (IsInactive) { return; }

    _state = WeaponState.Inactive;
    _shootingTimer.Stop();
  }
}
