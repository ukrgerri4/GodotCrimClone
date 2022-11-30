using Godot;
using System;

public class PlayerCamera : Camera
{
  private Configuration configuration;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    configuration = GetNode<Configuration>("/root/Configuration");
    configuration.OnMouseCaptionChanged += (CameraModeChangedEvent @event) =>
    {
      Current = @event.CameraMode == CameraMode.Player ? true : false;
    };

    var palyer = GetParent<Player>();

    // Translation = new Vector3(palyer.Translation.x, palyer.Translation.y + 20, palyer.Translation.z);
    // LookAt(palyer.Translation, Vector3.Up);
  }

  public override void _Input(InputEvent @event)
  {
    if (!Current) { return; }
  }

  //  // Called every frame. 'delta' is the elapsed time since the previous frame.
  //  public override void _Process(float delta)
  //  {
  //      
  //  }
}
