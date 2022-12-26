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
    zombieTemplate = GD.Load<PackedScene>("res://Scenes/Enemies/Zombie/Zombie.tscn");

    AddSphere(new Vector3(15, 1f, 15));
    // AddSphere(new Vector3(20, 1f, 0));
    // AddSphere(new Vector3(15, 1f, 5));
    // AddSphere(new Vector3(15, 1f, -5));
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

    private void AddSphere(Vector3 position)
  {
    var zombie = zombieTemplate.Instance<Zombie>();
    zombie.Translation = position;
    AddChild(zombie);
  }
}
