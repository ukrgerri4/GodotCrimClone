using Godot;
using System;
using System.Collections.Generic;

public class BulletEventManager : Node
{
  public Queue<Bullet> FreeBullets { get; set; } = new Queue<Bullet>();

  public override void _Ready()
  {
    var bulletTemplate = GD.Load<PackedScene>("res://Scenes/Components/Bullet.tscn");

    for (int i = 0; i < 1000; i++)
    {
      FreeBullets.Enqueue(bulletTemplate.Instance<Bullet>());
    }
  }
}