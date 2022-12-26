using Godot;
using System.Linq;

public class Player : KinematicBody
{
  public delegate void WeaponChanged(WeaponChangedEvent @event);
  public event WeaponChanged OnWeaponChanged;
  public int JoyId { get; set; } = 0;

  private WeaponsWarehouse _weaponsWarehouse;
  private CursorEventManager _cursorEventManager;

  private Weapon _weapon;
  private PerspectiveCamera _perspectiveCamera;
  private OrthogonalCamera _orthogonalCamera;
  private Vector3 _lookAtPosition;

  private bool IsMouseModeVisible => Input.MouseMode == Input.MouseModeEnum.Visible;
  private float CameraRotation => _perspectiveCamera.Current ? _perspectiveCamera.GlobalRotation.y : _orthogonalCamera.GlobalRotation.y;

  public override void _Ready()
  {
    _lookAtPosition = new Vector3(0, Translation.y, 0);
    _weaponsWarehouse = GetNode<WeaponsWarehouse>("/root/WeaponsWarehouse");
    EquipWeapon();

    _cursorEventManager = GetNode<CursorEventManager>("/root/CursorEventManager");
    _cursorEventManager.AddPositionChangedHandler((CursorPositionChangedEvent @event) =>
    {
      _lookAtPosition = new Vector3(@event.Position.x, Translation.y, @event.Position.z);
    });

    _perspectiveCamera = GetNode<PerspectiveCamera>("PerspectiveCamera");
    _orthogonalCamera = GetNode<OrthogonalCamera>("OrthogonalCamera");
  }

  public override void _PhysicsProcess(float delta)
  {
    if (Input.MouseMode.IsVisible())
    {
      Move(delta);
      if (GlobalTranslation.x != _lookAtPosition.x && GlobalTranslation.z != _lookAtPosition.z)
      {
        LookAt(_lookAtPosition, Vector3.Up);
      }
    }
    else
    {
      Move3(delta);
      LookByJoy(delta);
    }
  }

  public override void _Input(InputEvent @event)
  {
    if (Input.IsActionJustPressed("next_weapon"))
    {
      NextWeapon();
      return;
    }

    if (Input.IsActionPressed("fire"))
    {
      _weapon.StartShooting();
    }
    else if (Input.IsActionJustReleased("fire"))
    {
      _weapon.StopShooting();
    }
  }

  private void Move(float delta)
  {
    var moveX = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
    var moveZ = Input.GetActionStrength("move_backward") - Input.GetActionStrength("move_forward");

    if (moveX != 0 || moveZ != 0)
    {
      var motion = new Vector3(moveX, 0, moveZ);

      motion = motion.Normalized().Rotated(Vector3.Up, CameraRotation);

      MoveAndSlide(motion * 10);
    }
  }

  private void Move2(float delta)
  {
    Vector2 velocity = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");

    if (velocity.LengthSquared() > 0)
    {
      var motion = new Vector3(velocity.x, 0, velocity.y);

      motion = motion.Normalized().Rotated(Vector3.Up, CameraRotation);
      motion.y = Translation.y;

      MoveAndSlide(motion * 10);
    }
  }

  private void Move3(float delta)
  {
    Vector2 velocity = new Vector2(
      Input.GetJoyAxis(JoyId, (int)JoystickList.Axis0),
      Input.GetJoyAxis(JoyId, (int)JoystickList.Axis1)
    );

    if (velocity.LengthSquared() > 0.05)
    {
      var motion = new Vector3(velocity.x, 0, velocity.y);

      motion = motion.Normalized().Rotated(Vector3.Up, CameraRotation);
      motion.y = Translation.y;

      MoveAndSlide(motion * 10);
    }
  }

  private void LookByJoy(float delta)
  {
    // Vector2 velocity = Input.GetVector("look_left", "look_right", "look_forward", "look_backward");
    Vector2 velocity = new Vector2(
      Input.GetJoyAxis(JoyId, (int)JoystickList.Axis2),
      Input.GetJoyAxis(JoyId, (int)JoystickList.Axis3)
    );

    // used LengthSquared() because it runs faster than Length()
    // TODO: improve aiming!!!
    if (velocity.LengthSquared() > 0.9f)
    {
      var angle = velocity.Rotated(Mathf.Deg2Rad(90)).Rotated(CameraRotation).Angle();
      Rotation = new Vector3(Rotation.x, Mathf.LerpAngle(Rotation.y, -1 * angle, 0.2f), Rotation.z);
      // Rotation = new Vector3(Rotation.x, -1 * angle, Rotation.z);
    }
    else if (velocity.LengthSquared() > 0.25f)
    {
      var angle = velocity.Rotated(Mathf.Deg2Rad(90)).Rotated(CameraRotation).Angle();
      Rotation = new Vector3(Rotation.x, Mathf.LerpAngle(Rotation.y, -1 * angle, 0.7f), Rotation.z);
      // Rotation = new Vector3(Rotation.x, -1 * angle, Rotation.z);
    }
  }

  private void EquipWeapon()
  {
    var weapon = _weaponsWarehouse.CurrentWeapon;
    ChangeWeapon(weapon);
  }

  private void NextWeapon()
  {
    var weapon = _weaponsWarehouse.GetNextWeapon();
    ChangeWeapon(weapon);
  }

  private void ChangeWeapon(Weapon weapon)
  {
    weapon.Translation = new Vector3(0, 0, -0.25f);

    if (_weapon != null)
    {
      _weapon.StopShooting();
    }

    _weapon = weapon;

    if (!_weapon.IsInsideTree())
    {
      AddChild(_weapon);
    }

    OnWeaponChanged?.Invoke(new WeaponChangedEvent(weapon.GetType().Name));
  }
}
