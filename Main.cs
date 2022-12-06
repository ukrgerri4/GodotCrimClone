using Godot;
using System;

public class Main : Spatial
{
  private Configuration configuration;
  private PackedScene sphereTemplate;
  private int count = 0;

  public override void _Ready()
  {
    configuration = GetNode<Configuration>("/root/Configuration");
    sphereTemplate = GD.Load<PackedScene>("res://Scenes/SphereObject.tscn");
  }

  public override void _Process(float delta)
  {
    count++;
    if (count % 30 == 0)
    {
      AddSphere();
      count = 0;
    }
  }
  private void AddSphere()
  {
    var sphere = sphereTemplate.Instance<KinematicBody>();
    float angle = GD.Randf() * Mathf.Pi * 2;
    sphere.Translation = new Vector3(Mathf.Cos(angle) * 150, 1f, Mathf.Sin(angle) * 150);
    AddChild(sphere);
  }
}
