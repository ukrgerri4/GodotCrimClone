using Godot;
using System;

public class Bullet : KinematicBody
{
  public Vector3 Direction = Vector3.Zero;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {

  }

  // // Called every frame. 'delta' is the elapsed time since the previous frame.
  // public override void _Process(float delta)
  // {
  //   if (direction != Vector3.Zero)
  //   {
  //     MoveAndCollide(direction);
  //   }
  // }

  public override void _PhysicsProcess(float delta)
  {
    var collisionInfo = MoveAndCollide(Direction * delta * 5);
    if (collisionInfo != null)
    {
      if (collisionInfo.Collider is SphereObject sphere)
      {
        sphere.QueueFree();
      }
      
      QueueFree();
    }
  }
}
