using System;
using Godot;

public class Info : VBoxContainer
{
  private Configuration _configuration;
  private CursorEventManager _cursorEventManager;
  private System.Timers.Timer _timer;

  private Label _cameraModeLabel;
  private Label _mouseModeLabel;
  private Label _fpsLabel;
  private Label _mouseMovementLabel;
  private Label _weaponLabel;
  private Player _player;

  public override void _Ready()
  {
    _fpsLabel = GetNode<Label>("Fps");
    _cameraModeLabel = GetNode<Label>("CameraMode");
    _mouseModeLabel = GetNode<Label>("MouseMode");
    _mouseMovementLabel = GetNode<Label>("MouseMovement");
    _weaponLabel = GetNode<Label>("Weapon");

    InitFpsTimer();
    InitConfiguration();
    InitPlayerEvents();
    // InitCursorEventManager();
  }

  private void InitFpsTimer()
  {
    _fpsLabel.Text = $"FPS: {Engine.GetFramesPerSecond()}";

    _timer = new System.Timers.Timer(1000);
    _timer.Interval = 1000;
    _timer.AutoReset = true;
    _timer.Enabled = true;
    _timer.Elapsed += (object source, System.Timers.ElapsedEventArgs e) =>
    {
      _fpsLabel.Text = $"FPS: {Engine.GetFramesPerSecond()}";
    };
    _timer.Start();
  }

  private void InitConfiguration()
  {
    _configuration = GetNode<Configuration>("/root/Configuration");
    _configuration.OnMouseCaptionChanged += (CameraModeChangedEvent @event) =>
    {
      _cameraModeLabel.Text = $"CameraMode: {@event.CameraMode}";
    };
    _cameraModeLabel.Text = $"CameraMode: {_configuration.CameraMode}";

    _configuration.OnMouseModeChanged += (MouseModeChangedEvent @event) =>
    {
      _mouseModeLabel.Text = $"MouseMode: {@event.MouseMode}";
    };
    _mouseModeLabel.Text = $"MouseMode: {Input.MouseMode}";

  }

  private void InitPlayerEvents()
  {
    _player = GetNode<Player>("/root/Main/Player");
    _player.OnWeaponChanged += (WeaponChangedEvent @event) =>
    {
      _weaponLabel.Text = $"Weapon: {@event.WeaponName}";
    };
  }

  private void InitCursorEventManager()
  {
    _cursorEventManager = GetNode<CursorEventManager>("/root/CursorEventManager");
    _cursorEventManager.AddPositionChangedHandler(
      (CursorPositionChangedEvent @event) =>
      {
        _mouseMovementLabel.Text = $"Position: {@event.Position}";
      }
    );
  }
}
