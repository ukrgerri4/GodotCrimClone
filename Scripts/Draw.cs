using Godot;

public class Draw : ImmediateGeometry
{
  public override void _Ready()
  {
    SetAsToplevel(true);
    base._Ready();
  }

  public void DrawLine(Vector3 from, Vector3 to)
  {
    Begin(Mesh.PrimitiveType.LineStrip);
    AddVertex(ToLocal(from));
    AddVertex(ToLocal(to));
    End();
  }
}
