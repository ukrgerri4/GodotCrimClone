using Godot;
using System;
using System.Threading.Tasks;

public class Configuration : Node
{
  public delegate void MouseCaptionChanged(CameraModeChangedEvent cameraMode);
  public event MouseCaptionChanged OnMouseCaptionChanged;
  private CameraMode _cameraMode;
  public PlayersOptions PlayersOptions { get; set; }

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

  public delegate void MouseModeChanged(MouseModeChangedEvent cameraMode);
  public event MouseModeChanged OnMouseModeChanged;


  public float MouseSensitivity { get; set; } = 0.002f;

  public BulletConfigurations Bullets { get; set; }

  public override void _Ready()
  {
    InitPlayerDefaults();

    Input.Singleton.Connect("joy_connection_changed", this, nameof(OnJoyConnectionChanged));

    if (OS.IsDebugBuild())
    {
      var screenSize = OS.GetScreenSize(0);
      OS.WindowPosition = new Vector2(
        screenSize.x / 2 - OS.WindowSize.x / 2,
        screenSize.y / 2 - OS.WindowSize.y / 2 - 200
      );
    }

    Task.Run(async () =>
    {
      await ToSignal(GetTree().Root, "ready");

      Input.MouseMode = Input.MouseModeEnum.Visible;

      CameraMode = CameraMode.PlayerPerspective;

      Bullets = new BulletConfigurations
      {
        DefaultSpeed = 30f,
        MaxExistingTimeSec = 3f,
        DeviationDegrees = 1,
        DeviationRadians = Mathf.Deg2Rad(1)
      };
    });
  }

  private void InitPlayerDefaults()
  {
    PlayersOptions = new PlayersOptions
    {
      Player0 = new PlayerDefaultOptions
      {
        Name = "Trooper0",
        AppearPosition = new Vector3(0, 1, 0),
        JoyPadId = -1
      },
      Player1 = new PlayerDefaultOptions
      {
        Name = "Trooper1",
        AppearPosition = new Vector3(3, 1, 0),
        JoyPadId = 0
      },
      Player2 = new PlayerDefaultOptions
      {
        Name = "Trooper2",
        AppearPosition = new Vector3(0, 1, 3),
        JoyPadId = 1
      },
      Player3 = new PlayerDefaultOptions
      {
        Name = "Trooper3",
        AppearPosition = new Vector3(-3, 1, 0),
        JoyPadId = 2
      },
      Player4 = new PlayerDefaultOptions
      {
        Name = "Trooper4",
        AppearPosition = new Vector3(0, 1, -3),
        JoyPadId = 3
      }
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

    if (Input.IsActionJustPressed("full_screen"))
    {
      OS.WindowFullscreen = OS.WindowFullscreen ? false : true;
    }
  }

  private void ToggleMouseMode()
  {
    Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured
      ? Input.MouseModeEnum.Visible
      : Input.MouseModeEnum.Captured;

    OnMouseModeChanged?.Invoke(new MouseModeChangedEvent(Input.MouseMode));
  }

  private void ToggleCameraMode()
  {
    var nextCameraMode = ((int)CameraMode + 1) % 3;
    CameraMode = (CameraMode)nextCameraMode;
  }

  private void OnJoyConnectionChanged(int device, bool connected, string name, string guid)
  {
    // TODO: implement
  }
}