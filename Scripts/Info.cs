using Godot;
using System;

public class Info : Label
{
  private CursorEventManager _cursorEventManager;

  public override void _Ready()
  {
    _cursorEventManager = GetNode<CursorEventManager>("/root/CursorEventManager");

    _cursorEventManager.AddPositionChangedHandler(
      (CursorPositionChangedEvent @event) =>
      {
        Text = $"Position: {@event.Position}\nNormal: {@event.Normal}\nShapeId: {@event.ShapeIdx}";
      }
    );
  }
}
