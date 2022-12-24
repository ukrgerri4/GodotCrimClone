using Godot;
using System;

public class OrthogonalCamera : Camera
{
  private Configuration _configuration;

  public override void _Ready()
  {
    SetAsToplevel(true);

    _configuration = GetNode<Configuration>("/root/Configuration");
    _configuration.OnMouseCaptionChanged += (CameraModeChangedEvent @event) =>
    {
      Current = @event.CameraMode == CameraMode.PlayerOrthogonal ? true : false;
    };
  }
}
