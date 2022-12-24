using Godot;

public class PlasmaGun : Weapon
{
  public PlasmaGun() : base(50, 10, 5, 1000, 60) { }

  protected override void HandleShoot()
  {
    var bullet = _bulletEventManager.DequeuePlasmaGunBullet();

    if (!bullet.IsInsideTree())
    {
      _main.AddChild(bullet);
    }

    bullet.Enable(
      _bulletEntryPoint.GlobalTranslation,
      -1 * new Vector3(GlobalTranslation.x, _bulletEntryPoint.GlobalTranslation.y, GlobalTranslation.z) + _bulletEntryPoint.GlobalTranslation,
      splitable: true
    );
  }
}
