using Godot;
using System;

public class Player : KinematicBody
{
  private Configuration configuration;
  private BulletEventManager bulletEventManager;

  private PlayerCamera playerCamera;
  private PackedScene bulletTemplate;
  private Position3D bulletEntryPoint;

  private bool IsMouseModeVisible => Input.MouseMode == Input.MouseModeEnum.Visible;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    playerCamera = GetNode<PlayerCamera>("Camera");
    bulletEntryPoint = GetNode<Position3D>("BulletEntryPoint");
    bulletTemplate = GD.Load<PackedScene>("res://Scenes/Bullet.tscn");

    configuration = GetNode<Configuration>("/root/Configuration");
    bulletEventManager = GetNode<BulletEventManager>("/root/BulletEventManager");

    configuration.OnMouseCaptionChanged += (CameraModeChangedEvent @event) =>
    {
      playerCamera.Current = @event.CameraMode == CameraMode.Player ? true : false;
    };
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {

  }

  public override void _PhysicsProcess(float delta)
  {
    if (configuration.CameraMode == CameraMode.Player)
    {
      var motion = new Vector3(
          Input.GetActionStrength("move_forward") - Input.GetActionStrength("move_backward"),
          0,
          Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left")
      );

      MoveAndSlide(motion * 10);
    }

    if (Input.IsActionPressed("ui_accept"))
    {
      bulletEventManager.AddBullet(bulletEntryPoint.GlobalTranslation);
      // Bullet bullet = bulletTemplate.Instance<Bullet>();
      // bullet.Translation = new Vector3(bulletEntryPoint.Translation);
      // bullet.Direction = new Vector3(bulletEntryPoint.Translation);
      // AddChild(bullet);
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
}
