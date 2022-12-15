using Godot;
using System;

public class HealthBar3DViewport : Spatial
{
  private ProgressBar _healthBar2D;

  public float MaxValue = 0;

  public override void _Ready()
  {
    _healthBar2D = GetNode<ProgressBar>("Viewport/HealthBar2D");
    _healthBar2D.MaxValue = MaxValue;
    _healthBar2D.Value = MaxValue;
  }

  public void Update(float value)
  {
    _healthBar2D.Value = value;
  }
}
