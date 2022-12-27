using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Godot;

public class Main : Spatial
{
  private Configuration _configuration;
  private PackedScene _zombieTemplate;
  private PackedScene _playerTemplate;
  private List<Player> _players;

  private int count = 0;
  public override void _Ready()
  {
    _configuration = GetNode<Configuration>("/root/Configuration");
    _zombieTemplate = GD.Load<PackedScene>("res://Scenes/Enemies/Zombie/Zombie.tscn");
    _playerTemplate = GD.Load<PackedScene>("res://Scenes/Players/Player.tscn");
    _players = new List<Player>();

    // AddSphere(new Vector3(15, 1f, 15));
    // AddSphere(new Vector3(20, 1f, 0));
    // AddSphere(new Vector3(15, 1f, 5));
    // AddSphere(new Vector3(15, 1f, -5));

    CallDeferred(nameof(AddPlayerByJoypadId), -1);

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
      GD.Print(joypadButtonEvent.Device);
      if (!_players.Any(x => x.JoyPadId == joypadButtonEvent.Device))
      {
        CallDeferred(nameof(AddPlayerByJoypadId), joypadButtonEvent.Device);
      }
    }
  }

  private void AddSphere()
  {
    var zombie = _zombieTemplate.Instance<KinematicBody>();
    float angle = GD.Randf() * Mathf.Pi * 2;
    zombie.Translation = new Vector3(Mathf.Cos(angle) * 75, 1f, Mathf.Sin(angle) * 75);
    AddChild(zombie);
  }

  private void AddSphere(Vector3 position)
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
}
