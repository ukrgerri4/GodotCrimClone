using Godot;
using System;
using System.Collections.Generic;

public class BulletEventManager : Node
{
  private PackedScene _machineGunBulletTemplate;
  private PackedScene _plasmaGunBulletTemplate;

  private Queue<MachineGunBullet> _machineGunBullets { get; set; } = new Queue<MachineGunBullet>();
  private Queue<PlasmaGunBullet> _plasmaGunBullets { get; set; } = new Queue<PlasmaGunBullet>();

  public override void _Ready()
  {
    _machineGunBulletTemplate = GD.Load<PackedScene>("res://Scenes/Components/Bullets/MachineGunBullet/MachineGunBullet.tscn");
    _plasmaGunBulletTemplate = GD.Load<PackedScene>("res://Scenes/Components/Bullets/PlasmaGunBullet/PlasmaGunBullet.tscn");

    for (int i = 0; i < 200; i++)
    {
      _machineGunBullets.Enqueue(_machineGunBulletTemplate.Instance<MachineGunBullet>());
    }

    for (int i = 0; i < 50; i++)
    {
      _plasmaGunBullets.Enqueue(_plasmaGunBulletTemplate.Instance<PlasmaGunBullet>());
    }
  }

  public MachineGunBullet DequeueMachineGunBullet()
  {
    if (_machineGunBullets.Count > 0)
    {
      return _machineGunBullets.Dequeue();
    }
    else
    {
      return _machineGunBulletTemplate.Instance<MachineGunBullet>();
    }
  }

  public void EnqueueBullet(MachineGunBullet bullet)
  {
    _machineGunBullets.Enqueue(bullet);
  }

  public PlasmaGunBullet DequeuePlasmaGunBullet()
  {
    if (_plasmaGunBullets.Count > 0)
    {
      return _plasmaGunBullets.Dequeue();
    }
    else
    {
      return _plasmaGunBulletTemplate.Instance<PlasmaGunBullet>();
    }
  }

  public void EnqueueBullet(PlasmaGunBullet bullet)
  {
    _plasmaGunBullets.Enqueue(bullet);
  }
}