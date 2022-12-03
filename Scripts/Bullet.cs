using Godot;

public class Bullet : Area
{
  private Configuration _configuration;
  private Timer _timer;

  public Vector3 Direction { get; set; } = Vector3.Zero;

  public override void _Ready()
  {
    _configuration = GetNode<Configuration>("/root/Configuration");

    Direction = Direction.Rotated(
      Vector3.Up,
      (float)GD.RandRange(
        -1 * _configuration.Bullets.DeviationRadians,
        _configuration.Bullets.DeviationRadians
        )
    );

    _timer = GetNode<Timer>("DisposeTimer");
    _timer.WaitTime = _configuration.Bullets.MaxExistingTimeSec;
    _timer.Connect("timeout", this, "Remove");
    _timer.Start();
  }

  public override void _PhysicsProcess(float delta)
  {
    MoveAsArea(delta);
    // MoveAsKinematicBody(delta)
  }

  private void _on_Bullet_body_entered(Node body)
  {
    if (body is SphereObject sphere)
    {
      sphere.QueueFree();
      Remove();
    }
  }

  private void Remove()
  {
    QueueFree();
  }

  private void MoveAsArea(float delta)
  {
    Translation += Direction * delta * _configuration.Bullets.DefaultSpeed;
  }

  // private void MoveAsKinematicBody(float delta)
  // {
  //   var collisionInfo = MoveAndCollide(Direction * delta * 5);
  //   if (collisionInfo != null)
  //   {
  //     if (collisionInfo.Collider is SphereObject sphere)
  //     {
  //       sphere.QueueFree();
  //       QueueFree();
  //     }
  //   }
  // }
}
