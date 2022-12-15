using Godot;
using System;
using System.Collections.Generic;

public class BulletEventManager : Node
{
  public Queue<Bullet> FreeBullets { get; set; } = new Queue<Bullet>();

  public override void _Ready()
  {
    var bulletTemplate = GD.Load<PackedScene>("res://Scenes/Components/Bullet.tscn");

    for (int i = 0; i < 100; i++)
    {
      FreeBullets.Enqueue(bulletTemplate.Instance<Bullet>());
    }
  }

  // TODO: add mechanism to create new bullets and add to queue in case of bullet lack
}