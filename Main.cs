using Godot;
using System;

public class Main : Spatial
{
  private Configuration configuration;
  private PackedScene zombieTemplate;
  private int count = 0;

  public override void _Ready()
  {
    configuration = GetNode<Configuration>("/root/Configuration");
    zombieTemplate = GD.Load<PackedScene>("res://Scenes/Enemies/Zombie.tscn");
  }

  public override void _Process(float delta)
  {
    count++;
    if (count % 10 == 0)
    {
      AddSphere();
      count = 0;
    }
  }
  private void AddSphere()
  {
    var zombie = zombieTemplate.Instance<KinematicBody>();
    float angle = GD.Randf() * Mathf.Pi * 2;
    zombie.Translation = new Vector3(Mathf.Cos(angle) * 75, 1f, Mathf.Sin(angle) * 75);
    AddChild(zombie);
  }
}
