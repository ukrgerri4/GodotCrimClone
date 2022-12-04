using Godot;
using System;
using System.Timers;

public class AutoShotgun2 : Weapon
{
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
  private BulletEventManager _bulletEventManager;
  private Position3D _bulletEntryPoint;
  private System.Timers.Timer _shootingTimer;
  private bool shootReady = true;

  public override void _Ready()
  {
    _bulletEventManager = GetNode<BulletEventManager>("/root/BulletEventManager");
    _bulletEntryPoint = GetNode<Position3D>("BulletEntryPoint");

    RateOfFirePerSecond = 15;

    _shootingTimer = new System.Timers.Timer();
    _shootingTimer.Elapsed += OnTimeout;
    _shootingTimer.Enabled = true;
    _shootingTimer.Interval = _minShootingIntervalMilliseconds;
    _shootingTimer.AutoReset = false;
    _shootingTimer.Start();
  }

  private void OnTimeout(object sender, ElapsedEventArgs e)
  {
    shootReady = true;
  }

  // public override void Shoot()
  // {
  //   if (shootReady)
  //   {
  //     _bulletEventManager.AddBullet(
  //       new ShotEvent
  //       {
  //         EntryPoint = _bulletEntryPoint.GlobalTranslation,
  //         Direction = -1 * GlobalTranslation + _bulletEntryPoint.GlobalTranslation
  //       }
  //     );

  //     shootReady = false;
  //     _shootingTimer.Start();
  //   }
  // }

  private void ShootingProcess()
  {
    while(shootReady)
    {
      _bulletEventManager.AddBullet(
        new ShotEvent
        {
          EntryPoint = _bulletEntryPoint.GlobalTranslation,
          Direction = -1 * GlobalTranslation + _bulletEntryPoint.GlobalTranslation
        }
      );
    }
  }

  public override void StartShooting()
  {
    throw new NotImplementedException();
  }

  public override void StopShooting()
  {
    throw new NotImplementedException();
  }
}
