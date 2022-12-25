using Godot;

public class CameraModeChangedEvent
{
  public CameraModeChangedEvent(CameraMode cameraMode)
  {
    CameraMode = cameraMode;
  }

  public CameraMode CameraMode { get; private set; }
}