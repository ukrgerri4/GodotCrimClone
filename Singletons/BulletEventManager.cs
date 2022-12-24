using Godot;
using System;
using System.Collections.Generic;

public class BulletEventManager : Node
{
  private PackedScene _machineGunBulletTemplate;
  private PackedScene _laserGunBulletTemplate;
  private PackedScene _plasmaGunBulletTemplate;

  private Queue<MachineGunBullet> _machineGunBullets { get; set; } = new Queue<MachineGunBullet>();
  private Queue<LaserGunBullet> _laserGunBullets { get; set; } = new Queue<LaserGunBullet>();
  private Queue<PlasmaGunBullet> _plasmaGunBullets { get; set; } = new Queue<PlasmaGunBullet>();

  public override void _Ready()
  {
    _machineGunBulletTemplate = GD.Load<PackedScene>("res://Scenes/Weapons/MachineGun/MachineGunBullet/MachineGunBullet.tscn");
    _laserGunBulletTemplate = GD.Load<PackedScene>("res://Scenes/Weapons/LaserGun/LaserGunBullet/LaserGunBullet.tscn");
    _plasmaGunBulletTemplate = GD.Load<PackedScene>("res://Scenes/Weapons/PlasmaGun/PlasmaGunBullet/PlasmaGunBullet.tscn");

    for (int i = 0; i < 200; i++)
    {
      _machineGunBullets.Enqueue(_machineGunBulletTemplate.Instance<MachineGunBullet>());
    }

    for (int i = 0; i < 20; i++)
    {
      _laserGunBullets.Enqueue(_laserGunBulletTemplate.Instance<LaserGunBullet>());
    }

    for (int i = 0; i < 400; i++)
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

  public LaserGunBullet DequeueLaserGunBullet()
  {
    if (_laserGunBullets.Count > 0)
    {
      return _laserGunBullets.Dequeue();
    }
    else
    {
      return _laserGunBulletTemplate.Instance<LaserGunBullet>();
    }
  }

  public void EnqueueBullet(LaserGunBullet bullet)
  {
    _laserGunBullets.Enqueue(bullet);
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