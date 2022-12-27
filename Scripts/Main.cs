using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Godot;

public class Main : Spatial
{
  private Configuration _configuration;
  private WorldEnvironment _worldEnvironment;
  private PackedScene _zombieTemplate;
  private PackedScene _playerTemplate;
  private List<Player> _players;
  private int count = 0; // TMP
  private Tween _tween;

  public override void _Ready()
  {
    _configuration = GetNode<Configuration>("/root/Configuration");
    _worldEnvironment = GetNode<WorldEnvironment>("Environment");
    _tween = GetNode<Tween>("Tween");
    _zombieTemplate = GD.Load<PackedScene>("res://Scenes/Enemies/Zombie/Zombie.tscn");
    _playerTemplate = GD.Load<PackedScene>("res://Scenes/Players/Player.tscn");
    _players = new List<Player>();

    CallDeferred(nameof(AddPlayerByJoypadId), -1);
    InitJoinPlayerTimer();
    InitBlurEffect();
  }

  // Time when other players can join game by pressing any joypad button
  private void InitJoinPlayerTimer()
  {
    var timer = new System.Timers.Timer();
    timer.Interval = TimeSpan.FromSeconds(10).TotalMilliseconds;
    timer.AutoReset = false;
    timer.Elapsed += (object sender, ElapsedEventArgs e) =>
    {
      SetProcessInput(false);
      timer.Dispose();
    };
    timer.Start();
  }

  // Blur effect on start 
  private void InitBlurEffect()
  {
    _worldEnvironment.Environment.DofBlurFarEnabled = true;
    _worldEnvironment.Environment.DofBlurFarAmount = 0.3f;
    _tween.InterpolateProperty(_worldEnvironment.Environment, "dof_blur_far_amount", _worldEnvironment.Environment.DofBlurFarAmount, 0, 2f, Tween.TransitionType.Quint, Tween.EaseType.InOut);
    _tween.Connect("tween_completed", this, nameof(OnTweenCompleted));
    _tween.Start();
  }

  public override void _Process(float delta)
  {
    count++;
    if (count % 10 == 0)
    {
      AddSphere();
      count = 0;
    }
  }

  public override void _Input(InputEvent @event)
  {
    if (@event is InputEventJoypadButton joypadButtonEvent)
    {
      if (!_players.Any(x => x.JoyPadId == joypadButtonEvent.Device))
      {
        CallDeferred(nameof(AddPlayerByJoypadId), joypadButtonEvent.Device);
      }
    }
  }

  private void AddSphere() // TMP
  {
    var zombie = _zombieTemplate.Instance<KinematicBody>();
    float angle = GD.Randf() * Mathf.Pi * 2;
    zombie.Translation = new Vector3(Mathf.Cos(angle) * 75, 1f, Mathf.Sin(angle) * 75);
    AddChild(zombie);
  }

  private void AddSphere(Vector3 position) // TMP
  {
    var zombie = _zombieTemplate.Instance<Zombie>();
    zombie.Translation = position;
    AddChild(zombie);
  }

  private void AddPlayerByJoypadId(int joypadId)
  {
    var playerOptions = _configuration.PlayersOptions.First(x => x.JoyPadId == joypadId);
    GD.Print(playerOptions.Name);
    var palyer = _playerTemplate.Instance<Player>();
    palyer.Translation = playerOptions.AppearPosition;
    palyer.JoyPadId = playerOptions.JoyPadId;
    AddChild(palyer);
    _players.Add(palyer);
  }

  public void OnTweenCompleted(Godot.Object obj, NodePath key)
  {
    //TODO: Tween can be Free as well, in case of performance
    _worldEnvironment.Environment.DofBlurFarEnabled = false;
  }
}
