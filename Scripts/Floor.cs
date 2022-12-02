using Godot;
using System;

public class Floor : StaticBody
{
  private CursorEventManager _cursorEventManager;
  public override void _Ready()
  {
    _cursorEventManager = GetNode<CursorEventManager>("/root/CursorEventManager");
  }

  private void _on_Floor_input_event(Camera camera, InputEvent @event, Vector3 position, Vector3 normal, int shape_idx)
  {
    if (camera is FreeCamera && @event is InputEventMouseMotion eventMouseMotion)
    {
      _cursorEventManager.NotifyPositionChanged(
        new CursorPositionChangedEvent
        {
          Position = position,
          Normal = normal,
          ShapeIdx = shape_idx
        }
      );
    }
  }
}
