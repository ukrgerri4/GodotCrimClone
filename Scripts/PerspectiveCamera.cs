using Godot;

public class PerspectiveCamera : Camera
{
  private Configuration _configuration;

  public override void _Ready()
  {
    SetAsToplevel(true);

    _configuration = GetNode<Configuration>("/root/Configuration");
    _configuration.OnMouseCaptionChanged += (CameraModeChangedEvent @event) =>
    {
      Current = @event.CameraMode == CameraMode.PlayerPerspective ? true : false;
    };

    LookAt(Vector3.Zero, Vector3.Up);
  }
}
