using Godot;
using System;

public class PruneArea : Area
{
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
				
		}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	private void _on_PruneArea_body_entered(object node) {
		if (node is SphereObject sphere) {
			sphere.QueueFree();
		}
	}
}
