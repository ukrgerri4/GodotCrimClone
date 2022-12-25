using Godot;
using System;

public class PerspectiveCamera : Camera
{
  private Configuration _configuration;
  private Player _palyer;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    SetAsToplevel(true);

    _configuration = GetNode<Configuration>("/root/Configuration");
    _configuration.OnMouseCaptionChanged += (CameraModeChangedEvent @event) =>
    {
      Current = @event.CameraMode == CameraMode.PlayerPerspective ? true : false;
    };

    _palyer = GetParent<Player>();

    // Translation = new Vector3(palyer.Translation.x, palyer.Translation.y + 20, palyer.Translation.z);
    LookAt(_palyer.Translation, Vector3.Up);
  }

  public override void _Input(InputEvent @event)
  {
    if (!Current) { return; }

    // LookAt(_palyer.LookUpPosition, Vector3.Up);
  }

  //  // Called every frame. 'delta' is the elapsed time since the previous frame.
  //  public override void _Process(float delta)
  //  {
  //      
  //  }
}
