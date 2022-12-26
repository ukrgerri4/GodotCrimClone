using Godot;
using System;

public class FreeCamera : Camera
{
  private Configuration configuration;
  private const float MOUSE_SENSITIVITY = 0.002f;
  private const float MOVE_SPEED = 15f;

  private Vector3 rotation = Vector3.Zero;
  private Vector3 velocity = Vector3.Zero;

  public override void _Ready()
  {
    configuration = GetNode<Configuration>("/root/Configuration");
    configuration.OnMouseCaptionChanged += (CameraModeChangedEvent @event) =>
    {
      Current = @event.CameraMode == CameraMode.FreeView ? true : false;
    };

    rotation = Rotation;
  }

  public override void _Input(InputEvent @event)
  {
    if (IsCameraMovementDisabled()) { return; }

    if (@event is InputEventMouseMotion mouseEvent)
    {
      rotation.y -= mouseEvent.Relative.x * MOUSE_SENSITIVITY;
      rotation.x = Mathf.Clamp(rotation.x - mouseEvent.Relative.y * MOUSE_SENSITIVITY, -1.57f, 1.57f);
      var transform = Transform;
      transform.basis = new Basis(rotation);
      Transform = transform;
    }
  }

  public override void _PhysicsProcess(float delta)
  {
    if (IsCameraMovementDisabled()) { return; }

    var motion = new Vector3(
        Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
        0,
        Input.GetActionStrength("move_backward") - Input.GetActionStrength("move_forward")
    );

    motion = motion.Normalized();

    velocity += MOVE_SPEED * delta * Transform.basis.Xform(motion);
    velocity *= 0.85f;
    Translation += velocity;
  }

  private bool IsCameraMovementDisabled()
  {
    return !Current || !Input.MouseMode.IsCuptured();
  }
}
