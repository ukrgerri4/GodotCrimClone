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

  public bool IsPlayerCameraMode => CameraMode == CameraMode.PlayerPerspective;
  public bool IsFreeViewCameraMode => CameraMode == CameraMode.FreeView;

  public float MouseSensitivity { get; set; } = 0.002f;

  public BulletConfigurations Bullets { get; set; }

  public override void _Ready()
  {
    if (OS.IsDebugBuild())
    {
      var screenSize = OS.GetScreenSize(0);
      OS.WindowPosition = new Vector2(
        screenSize.x / 2 - OS.WindowSize.x / 2,
        screenSize.y / 2 - OS.WindowSize.y / 2
      );
    }

    Input.MouseMode = Input.MouseModeEnum.Captured;
    CameraMode = CameraMode.FreeView;

    Bullets = new BulletConfigurations
    {
      DefaultSpeed = 30f,
      MaxExistingTimeSec = 3f,
      DeviationDegrees = 1,
      DeviationRadians = Mathf.Deg2Rad(1)
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

    if (Input.IsActionJustPressed("full_screen")){
      OS.WindowFullscreen = OS.WindowFullscreen ? false : true;
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
    var nextCameraMode = ((int)CameraMode + 1) % 3;
    CameraMode = (CameraMode)nextCameraMode;
  }
}