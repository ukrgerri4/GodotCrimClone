using Godot;

public class LaserGun : Weapon
{
  private PackedScene _laserGunBulletTemplate;

  public LaserGun() : base(150, 30, 10, 1000, 30)
  {
    _laserGunBulletTemplate = GD.Load<PackedScene>("res://Scenes/Weapons/LaserGun/LaserGunBullet/LaserGunBullet.tscn");
  }

  protected override void HandleShoot()
  {
    var bullet = _laserGunBulletTemplate.Instance<LaserGunBullet>();
    bullet.Translation = _bulletEntryPoint.GlobalTranslation;
    bullet.Direction = ((-1 * new Vector3(GlobalTranslation.x, _bulletEntryPoint.GlobalTranslation.y, GlobalTranslation.z) + _bulletEntryPoint.GlobalTranslation)).Normalized() * EffectiveRange;
    _main.AddChild(bullet);
  }
}
