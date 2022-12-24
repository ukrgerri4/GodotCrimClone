using Godot;
using Godot.Collections;

public class PlasmaGunBullet : MeshInstance
{
  private BulletEventManager _bulletEventManager;
  private PhysicsDirectSpaceState _directSpaceState;
  private Node _main;

  private Vector3 _direction;
  private float _speed = 20f;
  private float _deviationRadians = BulletDefaultOptions.DeviationRadians;
  private float _existingTimeSec = 5f;
  private float _damage = 500;
  private bool _splitable = false;
  private Array _exclude;

  public override void _Ready()
  {
    SetPhysicsProcess(false);
    Hide();
    _main = GetTree().Root.GetNode("Main");
    _bulletEventManager = GetNode<BulletEventManager>("/root/BulletEventManager");
    _directSpaceState = GetWorld().DirectSpaceState;
    SetAsToplevel(true);
  }

  public override void _PhysicsProcess(float delta)
  {
    _existingTimeSec -= delta;
    if (_existingTimeSec <= 0)
    {
      Disable();
    }

    Move(delta);

    var cylinder = new CylinderShape();
    cylinder.Height = 0.01f;
    cylinder.Radius = (Mesh as SphereMesh).Radius;
    var shapeQuery = new PhysicsShapeQueryParameters();
    shapeQuery.SetShape(cylinder);
    shapeQuery.Transform = Transform;
    shapeQuery.CollisionMask = 2;
    if (_exclude != null)
    {

      shapeQuery.Exclude = _exclude;
    }

    var result = _directSpaceState.IntersectShape(shapeQuery, maxResults: 1);
    // GD.Print(result);

    if (result.Count > 0)
    {
      Disable();
      if (result[0] is Godot.Collections.Dictionary dict && dict.Contains("collider") && dict["collider"] is Zombie zombie)
      {
        var zombieGlobalTranslation = zombie.GlobalTranslation;
        var zombieTransform = zombie.Transform;

        zombie.HandleHit(_damage);
        // GD.Print(_splitable);
        if (_splitable)
        {
          cylinder.Radius = 50f;
          shapeQuery.SetShape(cylinder);
          shapeQuery.CollisionMask = 2;
          shapeQuery.Exclude = new Godot.Collections.Array { dict["rid"] };
          shapeQuery.Transform = zombieTransform;
          var result2 = _directSpaceState.IntersectShape(shapeQuery);
          // GD.Print(result2.Count);
          if (result2.Count > 0)
          {
            for (int i = 0; i < result2.Count; i++)
            {
              var enemy = (result2[i] as Godot.Collections.Dictionary)["collider"] as Zombie;
              var bullet = _bulletEventManager.DequeuePlasmaGunBullet();
              if (!bullet.IsInsideTree())
              {
                _main.AddChild(bullet);
              }
              // GD.Print("AddBullet");

              bullet.Enable(
                zombieGlobalTranslation,
                // -1 * new Vector3(enemy.GlobalTranslation.x, zombieGlobalTranslation.y, enemy.GlobalTranslation.z) + zombieGlobalTranslation
                -1 * new Vector3(zombieGlobalTranslation.x, enemy.GlobalTranslation.y, zombieGlobalTranslation.z) + enemy.GlobalTranslation,
                exclude: new Godot.Collections.Array { dict["rid"] }
              );
            }
          }
        }
      }
    }
  }

  private void Move(float delta)
  {
    Translation += _direction * _speed * delta;
  }

  public void Enable(Vector3 entryPoint, Vector3 direction, bool splitable = false, Godot.Collections.Array exclude = null)
  {
    Translation = entryPoint;
    _direction = direction.Normalized();
    _splitable = splitable;
    _exclude = exclude;

    _existingTimeSec = 5f;

    SetPhysicsProcess(true);
    Show();
  }

  public void Disable()
  {
    SetPhysicsProcess(false);
    Hide();
    _exclude = null;
    _direction = Vector3.Zero;
    _bulletEventManager.EnqueueBullet(this);
  }
}
