using Godot;
using System.Linq;

public class Player : KinematicBody
{
  public delegate void WeaponChanged(WeaponChangedEvent @event);
  public event WeaponChanged OnWeaponChanged;

  private WeaponsWarehouse _weaponsWarehouse;
  private CursorEventManager _cursorEventManager;

  private Weapon _weapon;
  private PerspectiveCamera _perspectiveCamera;
  private OrthogonalCamera _orthogonalCamera;
  private Tween _tween;
  private Vector3 _lookAtPosition = Vector3.Zero;

  private bool IsMouseModeVisible => Input.MouseMode == Input.MouseModeEnum.Visible;
  private float CameraRotation => _perspectiveCamera.Current ? _perspectiveCamera.GlobalRotation.y : _orthogonalCamera.GlobalRotation.y;

  public override void _Ready()
  {
    _weaponsWarehouse = GetNode<WeaponsWarehouse>("/root/WeaponsWarehouse");
    EquipWeapon();

    _cursorEventManager = GetNode<CursorEventManager>("/root/CursorEventManager");
    _cursorEventManager.AddPositionChangedHandler((CursorPositionChangedEvent @event) =>
    {
      _lookAtPosition = new Vector3(@event.Position.x, Translation.y, @event.Position.z);
    });

    _perspectiveCamera = GetNode<PerspectiveCamera>("PerspectiveCamera");
    _orthogonalCamera = GetNode<OrthogonalCamera>("OrthogonalCamera");
    _tween = GetNode<Tween>("Tween");
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
      Move(delta);
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

    if (Input.IsActionPressed("ui_accept"))
    {
      _weapon.StartShooting();
    }
    else if (Input.IsActionJustReleased("ui_accept"))
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

  private void LookByJoy(float delta)
  {
    var lookX = Input.GetActionStrength("look_right") - Input.GetActionStrength("look_left");
    var lookZ = Input.GetActionStrength("look_backward") - Input.GetActionStrength("look_forward");

    if (lookX != 0 || lookZ != 0)
    {
      var a = new Vector2(lookX, lookZ).Rotated(Mathf.Deg2Rad(90)).Rotated(CameraRotation).Angle();
      var m = new Vector3(lookX, Translation.y, lookZ);
      // GD.Print($"[{lookX}:{lookZ}:{Mathf.Rad2Deg(a)}:{m}]");

      // LookAt(m, Vector3.Up);

      _tween.StopAll();
      _tween.InterpolateProperty(this, "rotation:y", Rotation.y, -1 * a, 0.05f, Tween.TransitionType.Quad, Tween.EaseType.Out);
      _tween.Start();

      // _lookAtPosition = new Vector3(
      //   Input.GetActionStrength("look_right") - Input.GetActionStrength("look_left"),
      //   Translation.y,
      //   Input.GetActionStrength("look_backward") - Input.GetActionStrength("look_forward")
      // );
      // Rotation = m;
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
