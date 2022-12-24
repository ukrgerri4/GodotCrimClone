using Godot;
using System.Collections.Generic;

public class WeaponsWarehouse : Node
{
  private LinkedList<Weapon> _weapons = new LinkedList<Weapon>();
  private LinkedListNode<Weapon> _currentWeaponNode;

  public Weapon CurrentWeapon => _currentWeaponNode.Value;

  public override void _Ready()
  {
    _weapons.AddFirst(GD.Load<PackedScene>("res://Scenes/Weapons/MachineGun/MachineGun.tscn").Instance<MachineGun>());
    _weapons.AddLast(GD.Load<PackedScene>("res://Scenes/Weapons/LaserGun/LaserGun.tscn").Instance<LaserGun>());
    _weapons.AddLast(GD.Load<PackedScene>("res://Scenes/Weapons/PlasmaGun/PlasmaGun.tscn").Instance<PlasmaGun>());

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