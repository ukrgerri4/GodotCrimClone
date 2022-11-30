using Godot;
using System;

public class Floor : StaticBody
{
    private Label _info;
    private Player _player;

    public override void _Ready()
    {
        _info = GetNode<Label>("/root/Main/Info");
        _player = GetNode<Player>("/root/Main/Player");
    }

    // public override void _Input(InputEvent @event)
    // {
    // }

    //  public override void _Process(float delta)
    //  {  
    //  }

    // public override void _PhysicsProcess(float delta)
    // {
    // }

    private int lastShape = -1;
    private void _on_Floor_input_event(Camera camera, InputEvent @event, Vector3 position, Vector3 normal, int shape_idx)
    {
        if (@event is InputEventMouseMotion eventMouseMotion)
        {
          _info.Text = $"Position: {position}\nNormal: {normal}\nShapeId: {shape_idx}";

          var lookAtPosition = new Vector3(position);
          lookAtPosition.y = _player.Translation.y;
          _player.LookAt(lookAtPosition, Vector3.Up);
        }

        // if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed && eventMouseButton.ButtonIndex == 1)
        // {
        //     _player.LookAt(position, Vector3.Up);

        //     _tween.RemoveAll();
        //     _tween.InterpolateProperty(
        //         _player,
        //         "translation",
        //         _player.Translation,
        //         position,
        //         _player.Translation.DistanceTo(position) / 25, 
        //         Tween.TransitionType.Linear,
        //         Tween.EaseType.Out
        //     );
        //     _tween.Start();
        // }
    }
}
