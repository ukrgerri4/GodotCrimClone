using Godot;

public abstract class Weapon: Spatial
{
  public abstract void StartShooting();
  public abstract void StopShooting();
}