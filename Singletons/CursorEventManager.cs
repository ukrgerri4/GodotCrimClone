using Godot;
using System;

public class CursorEventManager : Node
{
  public delegate void CursorMovedEvent(CursorPositionChangedEvent entryPoint);
  private event CursorMovedEvent OnPositionChanged;

  public void NotifyPositionChanged(CursorPositionChangedEvent @event)
  {
    OnPositionChanged?.Invoke(@event);
  }

  public void AddPositionChangedHandler(CursorMovedEvent handler)
  {
    OnPositionChanged += handler;
  }
}