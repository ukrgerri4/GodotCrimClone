using Godot;
using System;

public class Main : Spatial
{
  private Configuration configuration;
  private BulletEventManager bulletEventManager;
  private PackedScene sphereTemplate;
  private PackedScene bulletTemplate;
  private int count = 0;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    configuration = GetNode<Configuration>("/root/Configuration");
    bulletEventManager = GetNode<BulletEventManager>("/root/BulletEventManager");
    sphereTemplate = GD.Load<PackedScene>("res://Scenes/SphereObject.tscn");
    bulletTemplate = GD.Load<PackedScene>("res://Scenes/Bullet.tscn");

    bulletEventManager.OnBulletAdd(PlaceBullet);
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
    count++;
    if (count % 30 == 0)
    {
      AddSphere();
      count = 0;
    }
  }

  // private void AddSphere() {
  //   var sphere = sphereTemplate.Instance<RigidBody>();
  //   float angle = GD.Randf() * Mathf.Pi * 2;
  //   // sphere.Translate(new Vector3(Mathf.Cos(angle) * 15, 0.3f, Mathf.Sin(angle) * 15));
  //   sphere.Translation = new Vector3(Mathf.Cos(angle) * 150, 1f, Mathf.Sin(angle) * 150);
  //   var force = (Vector3.Zero - sphere.Translation).Normalized() * 1000;
  //   AddChild(sphere);
  //   sphere.AddCentralForce(force);
  // }

  private void AddSphere()
  {
    var sphere = sphereTemplate.Instance<KinematicBody>();
    float angle = GD.Randf() * Mathf.Pi * 2;
    // sphere.Translate(new Vector3(Mathf.Cos(angle) * 15, 0.3f, Mathf.Sin(angle) * 15));
    sphere.Translation = new Vector3(Mathf.Cos(angle) * 150, 1f, Mathf.Sin(angle) * 150);
    // var force = (Vector3.Zero - sphere.Translation).Normalized() * 1000;
    AddChild(sphere);
    // sphere.MoveAndSlide(Vector3.Zero);
  }

  private void PlaceBullet(ShotEvent @event)
  {
    Bullet bullet = bulletTemplate.Instance<Bullet>();
    bullet.Translation = new Vector3(@event.EntryPoint);
    bullet.Direction = new Vector3(@event.Direction.Normalized());
    CallDeferred("add_child", bullet);
  }
}
