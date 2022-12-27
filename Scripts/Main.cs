using Godot;

public class Main : Spatial
{
  private Configuration _configuration;
  private PackedScene _zombieTemplate;
  private PackedScene _playerTemplate;
  private int count = 0;

  public override void _Ready()
  {
    _configuration = GetNode<Configuration>("/root/Configuration");
    _zombieTemplate = GD.Load<PackedScene>("res://Scenes/Enemies/Zombie/Zombie.tscn");
    _playerTemplate = GD.Load<PackedScene>("res://Scenes/Players/Player.tscn");

    // AddSphere(new Vector3(15, 1f, 15));
    // AddSphere(new Vector3(20, 1f, 0));
    // AddSphere(new Vector3(15, 1f, 5));
    // AddSphere(new Vector3(15, 1f, -5));

    // var palyer = _playerTemplate.Instance<Player>();
    // palyer.Translation = new Vector3(0,1,0);
    // AddChild(palyer);

    // var palyer2 = _playerTemplate.Instance<Player>();
    // palyer2.Translation = new Vector3(3,1,3);
    // palyer2.JoyId = 1;
    // AddChild(palyer2);

    AddPlayer(_configuration.PlayersOptions.Player0);
    AddPlayer(_configuration.PlayersOptions.Player1);
    AddPlayer(_configuration.PlayersOptions.Player2);
    AddPlayer(_configuration.PlayersOptions.Player3);
    AddPlayer(_configuration.PlayersOptions.Player4);
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

  private void AddPlayer(PlayerDefaultOptions playerOptions)
  {
    var palyer = _playerTemplate.Instance<Player>();
    palyer.Translation = playerOptions.AppearPosition;
    palyer.JoyPadId = playerOptions.JoyPadId;
    AddChild(palyer);
  }
}
