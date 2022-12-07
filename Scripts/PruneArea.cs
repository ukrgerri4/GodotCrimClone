using Godot;
using System;

public class PruneArea : Area
{
	private void _on_PruneArea_body_entered(object node) {
		if (node is Zombie sphere) {
			sphere.QueueFree();
		}
	}
}
