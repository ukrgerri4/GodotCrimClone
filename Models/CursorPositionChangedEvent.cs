using Godot;

public class CursorPositionChangedEvent
{
  public Vector3 Position {get; set; }
  public Vector3 Normal {get; set; }
  public int ShapeIdx {get; set; }
}