using System.Collections.Generic;

public class Armory
{
  private LinkedList<Weapon> _weapons = new LinkedList<Weapon>();
  private LinkedListNode<Weapon> _currentWeaponNode;
  private WeaponsWarehouse _weaponsWarehouse;

  public Weapon CurrentWeapon => _currentWeaponNode.Value;

  public Armory(WeaponsWarehouse weaponsWarehouse)
  {
    _weaponsWarehouse = weaponsWarehouse;

    _weapons.AddFirst(_weaponsWarehouse.MachineGunTemplate.Instance<MachineGun>());
    _weapons.AddLast(_weaponsWarehouse.LaserGunTemplate.Instance<LaserGun>());
    _weapons.AddLast(_weaponsWarehouse.PlasmaGunTemplate.Instance<PlasmaGun>());
    _currentWeaponNode = _weapons.First;
  }

  public Weapon GetNextWeapon()
  {
    if (_currentWeaponNode.Next != null)
    {
      _currentWeaponNode = _currentWeaponNode.Next;
    }
    else
    {
      _currentWeaponNode = _weapons.First;
    }

    return CurrentWeapon;
  }
}