using Godot;

public class MachineGun : Weapon
{
  public MachineGun() : base(300, 150, 30, 500, 2) { }

  protected override void HandleShoot()
  {
    var bullet = _bulletEventManager.DequeueMachineGunBullet();

    if (!bullet.IsInsideTree())
    {
      AddChild(bullet);
    }

    bullet.Enable(
      _bulletEntryPoint.GlobalTranslation,
      -1 * new Vector3(GlobalTranslation.x, _bulletEntryPoint.GlobalTranslation.y, GlobalTranslation.z) + _bulletEntryPoint.GlobalTranslation
    );
  }
}
