using Godot;
using System;

public class HealthBar3D : MeshInstance
{
  
  public override void _Ready()
  {

  }

  public override void _PhysicsProcess(float delta)
  {
    LookAt(GetViewport().GetCamera().Translation, Vector3.Up);
  }
}
