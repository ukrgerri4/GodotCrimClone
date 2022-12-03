using Godot;

public class Bullet : KinematicBody
{
  public Vector3 Direction = Vector3.Zero;

  public override void _PhysicsProcess(float delta)
  {
    var collisionInfo = MoveAndCollide(Direction * delta * 5);
    if (collisionInfo != null)
    {
      if (collisionInfo.Collider is SphereObject sphere)
      {
        sphere.QueueFree();
        QueueFree();
      }
    }
  }
}
