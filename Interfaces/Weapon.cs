using Godot;

public abstract class Weapon : Spatial
{
  protected Node _main;
  [Export]
  protected NodePath BulletEntryPointPath;
  protected Position3D _bulletEntryPoint;
  protected BulletEventManager _bulletEventManager;

  public WeaponState State { get; set; }
  public bool IsShooting => State == WeaponState.Shooting;
  public bool IsInactive => State == WeaponState.Inactive;

  public int MaxAmmoCapacity { get; set; }
  public int AmmoCapacity { get; set; }
  public int MagazineCapacity { get; set; }
  public int EffectiveRange { get; set; }
  protected int _fireRate = 60;
  public int FireRate
  {
    get => _fireRate;
    set
    {
      _fireRate = value;
      _framesBetweenBullet = _fireRate;
    }
  }

  protected int _framesBetweenBullet = 0;
  protected int _framesToNextBullet = 0;

  protected Weapon(
  int maxAmmoCapacity,
  int ammoCapacity,
  int magazineCapacity,
  int effectiveRange,
  int fireRate)
  {
    MaxAmmoCapacity = maxAmmoCapacity;
    AmmoCapacity = ammoCapacity;
    MagazineCapacity = magazineCapacity;
    EffectiveRange = effectiveRange;
    if (fireRate < 1 || fireRate > 60)
    {
      throw new System.Exception($"Fire rate should be between 1 and 120, resived value = {fireRate}");
    }
    FireRate = fireRate;
  }

  public override void _Ready()
  {
    _main = GetTree().Root.GetNode("Main");
    _bulletEventManager = GetNode<BulletEventManager>("/root/BulletEventManager");
    _bulletEntryPoint = GetNode<Position3D>(BulletEntryPointPath);
  }

  public override void _PhysicsProcess(float delta)
  {
    if (_framesToNextBullet == 0)
    {
      if (IsShooting)
      {
        HandleShoot();
        _framesToNextBullet = _framesBetweenBullet;
      }
    }
    else
    {
      _framesToNextBullet--;
    }
  }

  protected virtual void HandleShoot() { }

  public virtual void StartShooting()
  {
    if (IsShooting) { return; }
    State = WeaponState.Shooting;
  }
  public virtual void StopShooting()
  {
    if (IsInactive) { return; }
    State = WeaponState.Inactive;
  }
}