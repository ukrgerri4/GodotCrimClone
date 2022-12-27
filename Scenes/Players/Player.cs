using Godot;

public class Player : KinematicBody
{
  public delegate void WeaponChanged(WeaponChangedEvent @event);
  public event WeaponChanged OnWeaponChanged;
  public int JoyPadId { get; set; } = 0;

  private Armory _armory;
  private CursorEventManager _cursorEventManager;

  private Weapon _weapon;
  private Vector3 _lookAtPosition;

  private bool IsMouseModeVisible => Input.MouseMode == Input.MouseModeEnum.Visible;
  private float CameraRotationY => GetViewport().GetCamera()?.GlobalRotation.y ?? 0;

  public override void _Ready()
  {
    _lookAtPosition = new Vector3(0, Translation.y, 0);
    _armory = new Armory(GetNode<WeaponsWarehouse>("/root/WeaponsWarehouse"));
    EquipWeapon();

    _cursorEventManager = GetNode<CursorEventManager>("/root/CursorEventManager");
    _cursorEventManager.AddPositionChangedHandler((CursorPositionChangedEvent @event) =>
    {
      _lookAtPosition = new Vector3(@event.Position.x, Translation.y, @event.Position.z);
    });
  }

  public override void _PhysicsProcess(float delta)
  {
    if (Input.MouseMode.IsVisible() && JoyPadId == -1)
    {
      MoveByKeyboard(delta);
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
    if (JoyPadId == -1)
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
    else
    {
      if (Input.IsJoyButtonPressed(JoyPadId, (int)JoystickList.Button6))
      {
        NextWeapon();
        return;
      }

      if (Input.IsJoyButtonPressed(JoyPadId, (int)JoystickList.Button7))
      {
        _weapon.StartShooting();
      }
      else
      {
        _weapon.StopShooting();
      }
    }
  }

  private void MoveUniversal(float delta)
  {
    Vector2 moveVelocity = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");

    if (moveVelocity.LengthSquared() > 0)
    {
      var motion = new Vector3(moveVelocity.x, 0, moveVelocity.y);

      motion = motion.Normalized().Rotated(Vector3.Up, CameraRotationY);
      motion.y = Translation.y;

      MoveAndSlide(motion * 10); // TODO: MAGIC VALUE
    }
  }

  private void MoveByKeyboard(float delta)
  {
    var moveVelocity = new Vector2();
    if (Input.IsKeyPressed((int)KeyList.A))
      moveVelocity.x = -1;
    else if (Input.IsKeyPressed((int)KeyList.D))
      moveVelocity.x = 1;

    if (Input.IsKeyPressed((int)KeyList.W))
      moveVelocity.y = -1;
    else if (Input.IsKeyPressed((int)KeyList.S))
      moveVelocity.y = 1;

    if (moveVelocity.LengthSquared() > 0)
    {
      var motion = new Vector3(moveVelocity.x, 0, moveVelocity.y);

      motion = motion.Normalized().Rotated(Vector3.Up, CameraRotationY);
      motion.y = Translation.y;

      MoveAndSlide(motion * 10); // TODO: MAGIC VALUE
    }
  }

  private void MoveByJoy(float delta)
  {
    Vector2 moveVelocity = new Vector2(
      Input.GetJoyAxis(JoyPadId, (int)JoystickList.Axis0),
      Input.GetJoyAxis(JoyPadId, (int)JoystickList.Axis1)
    );

    if (moveVelocity.LengthSquared() > 0.05) // TODO: MAGIC VALUE
    {
      var motion = new Vector3(moveVelocity.x, 0, moveVelocity.y);

      motion = motion.Normalized().Rotated(Vector3.Up, CameraRotationY);
      motion.y = Translation.y;

      MoveAndSlide(motion * 10);
    }
  }

  private void LookByJoy(float delta)
  {
    Vector2 lookVelocity = new Vector2(
      Input.GetJoyAxis(JoyPadId, (int)JoystickList.Axis2),
      Input.GetJoyAxis(JoyPadId, (int)JoystickList.Axis3)
    );

    // used LengthSquared() because it runs faster than Length()
    // TODO: improve aiming!!!
    if (lookVelocity.LengthSquared() > 0.9f)
    {
      // Mathf.Deg2Rad(90) = 1.570796
      var angle = lookVelocity.Rotated(Mathf.Deg2Rad(90)).Rotated(CameraRotationY).Angle();
      Rotation = new Vector3(Rotation.x, Mathf.LerpAngle(Rotation.y, -1 * angle, 0.2f), Rotation.z);
    }
    else if (lookVelocity.LengthSquared() > 0.25f)
    {
      var angle = lookVelocity.Rotated(Mathf.Deg2Rad(90)).Rotated(CameraRotationY).Angle();
      Rotation = new Vector3(Rotation.x, Mathf.LerpAngle(Rotation.y, -1 * angle, 0.7f), Rotation.z);
    }
  }

  private void EquipWeapon()
  {
    var weapon = _armory.CurrentWeapon;
    ChangeWeapon(weapon);
  }

  private void NextWeapon()
  {
    var weapon = _armory.GetNextWeapon();
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
      // AddChild(_weapon);
      CallDeferred("add_child", _weapon);
    }

    OnWeaponChanged?.Invoke(new WeaponChangedEvent(weapon.GetType().Name));
  }
}
