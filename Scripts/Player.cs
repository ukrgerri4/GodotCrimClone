using Godot;
using System;

public class Player : KinematicBody
{
  private CursorEventManager _cursorEventManager;
  private Weapon _weapon;
  private PerspectiveCamera _perspectiveCamera;
  private OrthogonalCamera _orthogonalCamera;
  private Vector3 _lookAtPosition = Vector3.Zero;

  private bool IsMouseModeVisible => Input.MouseMode == Input.MouseModeEnum.Visible;
  private float CameraRotation => _perspectiveCamera.Current ? _perspectiveCamera.GlobalRotation.y : _orthogonalCamera.GlobalRotation.y;

  public override void _Ready()
  {
    // _weapon = GD.Load<PackedScene>("res://Scenes/Weapons/AutoShotgun.tscn").Instance<AutoShotgun>();
    _weapon = GD.Load<PackedScene>("res://Scenes/Weapons/Laser.tscn").Instance<Laser>();
    _weapon.Translation = new Vector3(0, 0, -0.25f);
    AddChild(_weapon);

    _cursorEventManager = GetNode<CursorEventManager>("/root/CursorEventManager");
    _cursorEventManager.AddPositionChangedHandler((CursorPositionChangedEvent @event) =>
    {
      _lookAtPosition = new Vector3(@event.Position.x, Translation.y, @event.Position.z);
      // var lookAtPosition = new Vector3(@event.Position);
      // lookAtPosition.y = Translation.y;
      // LookAt(lookAtPosition, Vector3.Up);
    });

    _perspectiveCamera = GetNode<PerspectiveCamera>("PerspectiveCamera");
    _orthogonalCamera = GetNode<OrthogonalCamera>("OrthogonalCamera");
  }

  public override void _PhysicsProcess(float delta)
  {
    if (Input.MouseMode.IsVisible())
    {
      MoveByKeyboard(delta);
    }
    else
    {
      _lookAtPosition = new Vector3(
        Input.GetActionStrength("look_right") - Input.GetActionStrength("look_left"),
        Translation.y,
        Input.GetActionStrength("look_backward") - Input.GetActionStrength("look_forward")
      );
    }

    if (GlobalTranslation != _lookAtPosition)
    {
      LookAt(_lookAtPosition, Vector3.Up);
    }

  }
  public override void _Input(InputEvent @event)
  {
    if (Input.IsActionJustPressed("ui_accept"))
    {
      _weapon.StartShooting();
    }
    else if (Input.IsActionJustReleased("ui_accept"))
    {
      _weapon.StopShooting();
    }
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

    motion = motion.Normalized().Rotated(Vector3.Up, CameraRotation);

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
