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
  }

  public override void _PhysicsProcess(float delta)
  {
    if (Input.MouseMode.IsVisible())
    {
      Move(delta);
    }
    else
    {
      _lookAtPosition = new Vector3(
        Input.GetActionStrength("look_right") - Input.GetActionStrength("look_left"),
        Translation.y,
        Input.GetActionStrength("look_backward") - Input.GetActionStrength("look_forward")
      );
    }

    if (GlobalTranslation.x != _lookAtPosition.x && GlobalTranslation.z != _lookAtPosition.z)
    {
      LookAt(_lookAtPosition, Vector3.Up);
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
    var motion = new Vector3(
        Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
        0,
        Input.GetActionStrength("move_backward") - Input.GetActionStrength("move_forward")
    );

    motion = motion.Normalized().Rotated(Vector3.Up, CameraRotation);

    MoveAndSlide(motion * 10);
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
