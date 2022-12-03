using Godot;
using System;

public class Configuration : Node
{
  public delegate void MouseCaptionChanged(CameraModeChangedEvent cameraMode);
  public event MouseCaptionChanged OnMouseCaptionChanged;
  private CameraMode _cameraMode;
  public CameraMode CameraMode
  {
    get => _cameraMode;
    private set
    {
      _cameraMode = value;
      OnMouseCaptionChanged?.Invoke(new CameraModeChangedEvent(_cameraMode));
    }
  }

  public bool IsPlayerCameraMode => CameraMode == CameraMode.Player;
  public bool IsFreeViewCameraMode => CameraMode == CameraMode.FreeView;

  public float MouseSensitivity { get; set; } = 0.002f;

  public BulletConfigurations Bullets { get; set; }

  public override void _Ready()
  {
    Input.MouseMode = Input.MouseModeEnum.Captured;
    CameraMode = CameraMode.FreeView;

    Bullets = new BulletConfigurations
    {
      DefaultSpeed = 30f,
      MaxExistingTimeSec = 3f,
      DeviationDegrees = 5,
      DeviationRadians = Mathf.Deg2Rad(5)
    };
  }

  public override void _Input(InputEvent @event)
  {
    if (Input.IsActionJustPressed("ui_cancel"))
    {
      GetTree().Quit();
    }

    if (Input.IsActionJustPressed("change_mouse_caption"))
    {
      ToggleCameraMode();
    }

    if (Input.IsActionJustPressed("change_mouse_mode"))
    {
      ToggleMouseMode();
    }
  }

  private void ToggleMouseMode()
  {
    Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured
      ? Input.MouseModeEnum.Visible
      : Input.MouseModeEnum.Captured;
  }

  private void ToggleCameraMode()
  {
    CameraMode = CameraMode == CameraMode.FreeView ? CameraMode.Player : CameraMode.FreeView;
  }
}