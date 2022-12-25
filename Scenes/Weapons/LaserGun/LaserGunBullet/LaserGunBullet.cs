using Godot;
using Godot.Collections;

public class LaserGunBullet : Position3D
{
  private PhysicsDirectSpaceState _directSpaceState;
  private Draw _draw;

  public Vector3 Direction { get; set; }

  public override void _Ready()
  {
    _directSpaceState = GetWorld().DirectSpaceState;
    _draw = GetNode<Draw>("/root/Main/Draw");

    ProcessShoot(GlobalTranslation, Direction, damage: 150);
  }

  public void ProcessShoot(Vector3 from, Vector3 to, float damage, Array exclude = null)
  {
    if (exclude == null)
    {
      exclude = new Array();
    }
    var result = _directSpaceState.IntersectRay(from, to, exclude: exclude, collisionMask: 4);

    if (result != null && result.Count > 0)
    {

      _draw.DrawLine(from, (Vector3)result["position"]);
      if (result is Godot.Collections.Dictionary && result.Contains("collider") && result["collider"] is Zombie zombie)
      {
        var intersectPoint = (Vector3)result["position"];
        var healthPoint = zombie.HandleHit(damage);
        if (healthPoint < 0)
        {
          exclude.Add(result["rid"]);
          var leftDamage = Mathf.Abs(healthPoint);
          ProcessShoot(intersectPoint, to, leftDamage, exclude);
        }
        else
        {
          QueueFree();
          return;
        }
      }
    }
    else
    {
      _draw.DrawLine(from, to);
      QueueFree();
      return;
    }
  }
}
