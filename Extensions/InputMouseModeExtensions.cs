using Godot;

public static class InputMouseModeExtensions
{
  public static bool IsCuptured(this Input.MouseModeEnum mouseMode) => mouseMode == Input.MouseModeEnum.Captured;
  public static bool IsVisible(this Input.MouseModeEnum mouseMode) => mouseMode == Input.MouseModeEnum.Visible;

} 