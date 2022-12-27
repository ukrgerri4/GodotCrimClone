using Godot;

public class WeaponsWarehouse : Node
{
  public PackedScene MachineGunTemplate { get; private set; }
  public PackedScene LaserGunTemplate { get; private set; }
  public PackedScene PlasmaGunTemplate { get; private set; }

  public override void _Ready()
  {
    MachineGunTemplate = GD.Load<PackedScene>("res://Scenes/Weapons/MachineGun/MachineGun.tscn");
    LaserGunTemplate = GD.Load<PackedScene>("res://Scenes/Weapons/LaserGun/LaserGun.tscn");
    PlasmaGunTemplate = GD.Load<PackedScene>("res://Scenes/Weapons/PlasmaGun/PlasmaGun.tscn");
  }
}