using static Godot.Input;

public class MouseModeChangedEvent
{
  public MouseModeChangedEvent(MouseModeEnum mouseMode)
  {
    MouseMode = mouseMode;
  }

  public MouseModeEnum MouseMode { get; private set; }
}