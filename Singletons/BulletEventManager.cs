using Godot;
using System;

public class BulletEventManager : Node
{
  public delegate void CreateBulletEvent(Vector3 entryPoint);
  private event CreateBulletEvent CreateBullet;

  public override void _Ready()
  {
    
  }

  public void AddBullet(Vector3 entryPoint)
  {
    CreateBullet?.Invoke(entryPoint);
  }

  public void OnBulletAdd(CreateBulletEvent handler)
  {
    CreateBullet += handler;
  }
}