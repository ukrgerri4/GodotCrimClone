using Godot;
using System;

public class Player : KinematicBody
{
  private Configuration _configuration;
  private BulletEventManager _bulletEventManager;
  private CursorEventManager _cursorEventManager;
  private PlayerCamera _playerCamera;
  private PackedScene _bulletTemplate;
  private Position3D _bulletEntryPoint;

  private bool IsMouseModeVisible => Input.MouseMode == Input.MouseModeEnum.Visible;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    _playerCamera = GetNode<PlayerCamera>("Camera");
    _bulletEntryPoint = GetNode<Position3D>("BulletEntryPoint");
    _bulletTemplate = GD.Load<PackedScene>("res://Scenes/Bullet.tscn");

    _configuration = GetNode<Configuration>("/root/Configuration");
    _bulletEventManager = GetNode<BulletEventManager>("/root/BulletEventManager");
    _cursorEventManager = GetNode<CursorEventManager>("/root/CursorEventManager");

    _configuration.OnMouseCaptionChanged += (CameraModeChangedEvent @event) =>
    {
      _playerCamera.Current = @event.CameraMode == CameraMode.Player ? true : false;
    };

    _cursorEventManager.AddPositionChangedHandler((CursorPositionChangedEvent @event) =>
    {
      var lookAtPosition = new Vector3(@event.Position);
      lookAtPosition.y = Translation.y;
      LookAt(lookAtPosition, Vector3.Up);
    });
  }

  public override void _Process(float delta)
  {

  }

  private float limit = 0.2f;
  public override void _PhysicsProcess(float delta)
  {
    limit = limit - delta;
    if (Input.IsActionPressed("ui_accept"))
    {
      GD.Print(limit);
      if (limit <= 0)
      {
        _bulletEventManager.AddBullet(
          new ShotEvent
          {
            EntryPoint = _bulletEntryPoint.GlobalTranslation,
            Direction = -1 * GlobalTranslation + _bulletEntryPoint.GlobalTranslation
          }
        );
        limit = 0.2f;
      }
    }

    if (_configuration.IsFreeViewCameraMode && Input.MouseMode.IsVisible())
    {
      MoveByKeyboard(delta);
    }

  }
  public override void _Input(InputEvent @event)
  {
    // if (IsMouseModeVisible && @event is InputEventMouseMotion mouseEvent)
    // {
    //   GD.Print(mouseEvent.Relative);
    //   var rotation = new Vector3();
    //   rotation.y -= mouseEvent.Relative.x * MOUSE_SENSITIVITY;
    //   // rotation.x = Mathf.Clamp(rotation.x - mouseEvent.Relative.y * MOUSE_SENSITIVITY, -1.57f, 1.57f);
    //   var transform = Transform;
    //   transform.basis = new Basis(rotation);
    //   Transform = transform;
    // }
  }

  // private void MoveByJoy()
  // {
  //   var xAxis = Input.GetJoyAxis(JoyId, (int)JoystickList.Axis2);
  //   var zAxis = Input.GetJoyAxis(JoyId, (int)JoystickList.Axis3);
  //   xAxis = Mathf.Abs(xAxis) > 0.1 ? xAxis : 0;
  //   zAxis = Mathf.Abs(zAxis) > 0.1 ? zAxis : 0;

  //   if (xAxis == 0 && zAxis == 0) return;

  //   MoveAndCollide(new Vector2(xAxis, zAxis) * 10);
  // }

  private void MoveByKeyboard(float delta)
  {
    var motion = new Vector3(
        Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
        0,
        Input.GetActionStrength("move_backward") - Input.GetActionStrength("move_forward")
    );

    motion = motion.Normalized();

    MoveAndSlide(motion * 10);
  }

  private void MoveByMouseClick()
  {
    // if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed && eventMouseButton.ButtonIndex == 1)
    // {
    //     _player.LookAt(position, Vector3.Up);

    //     _tween.RemoveAll();
    //     _tween.InterpolateProperty(
    //         _player,
    //         "translation",
    //         _player.Translation,
    //         position,
    //         _player.Translation.DistanceTo(position) / 25, 
    //         Tween.TransitionType.Linear,
    //         Tween.EaseType.Out
    //     );
    //     _tween.Start();
    // }
  }
}
