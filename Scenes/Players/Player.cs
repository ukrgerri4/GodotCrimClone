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
  private Vector3 _lookAtPosition;

  private bool IsMouseModeVisible => Input.MouseMode == Input.MouseModeEnum.Visible;
  private float CameraRotationY => GetViewport().GetCamera()?.GlobalRotation.y ?? 0;

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
  }

  public override void _PhysicsProcess(float delta)
  {
    if (Input.MouseMode.IsVisible())
    {
      MoveUniversal(delta);
      if (GlobalTranslation.x != _lookAtPosition.x && GlobalTranslation.z != _lookAtPosition.z)
      {
        LookAt(_lookAtPosition, Vector3.Up);
      }
    }
    else
    {
      MoveByJoy(delta);
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

  private void MoveUniversal(float delta)
  {
    Vector2 velocity = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");

    if (velocity.LengthSquared() > 0)
    {
      var motion = new Vector3(velocity.x, 0, velocity.y);

      motion = motion.Normalized().Rotated(Vector3.Up, CameraRotationY);
      motion.y = Translation.y;

      MoveAndSlide(motion * 10);
    }
  }

  private void MoveByJoy(float delta)
  {
    Vector2 velocity = new Vector2(
      Input.GetJoyAxis(JoyId, (int)JoystickList.Axis0),
      Input.GetJoyAxis(JoyId, (int)JoystickList.Axis1)
    );

    if (velocity.LengthSquared() > 0.05)
    {
      var motion = new Vector3(velocity.x, 0, velocity.y);

      motion = motion.Normalized().Rotated(Vector3.Up, CameraRotationY);
      motion.y = Translation.y;

      MoveAndSlide(motion * 10);
    }
  }

  private void LookByJoy(float delta)
  {
    Vector2 velocity = new Vector2(
      Input.GetJoyAxis(JoyId, (int)JoystickList.Axis2),
      Input.GetJoyAxis(JoyId, (int)JoystickList.Axis3)
    );

    // used LengthSquared() because it runs faster than Length()
    // TODO: improve aiming!!!
    if (velocity.LengthSquared() > 0.9f)
    {
      // Mathf.Deg2Rad(90) = 1.570796
      var angle = velocity.Rotated(Mathf.Deg2Rad(90)).Rotated(CameraRotationY).Angle();
      Rotation = new Vector3(Rotation.x, Mathf.LerpAngle(Rotation.y, -1 * angle, 0.2f), Rotation.z);
      // Rotation = new Vector3(Rotation.x, -1 * angle, Rotation.z);
    }
    else if (velocity.LengthSquared() > 0.25f)
    {
      var angle = velocity.Rotated(Mathf.Deg2Rad(90)).Rotated(CameraRotationY).Angle();
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
