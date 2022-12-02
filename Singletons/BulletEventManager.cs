using Godot;
using System;

public class BulletEventManager : Node
{
  public delegate void CreateBulletEvent(ShotEvent @event);
  private event CreateBulletEvent CreateBullet;

  public void AddBullet(ShotEvent @event)
  {
    CreateBullet?.Invoke(@event);
  }

  public void OnBulletAdd(CreateBulletEvent handler)
  {
    CreateBullet += handler;
  }
}