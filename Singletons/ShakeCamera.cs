using Godot;

public class ShakeCamera : Node
{
  private OpenSimplexNoise _noise;
  private RandomNumberGenerator _random;
  private Camera _camera;
  private Vector3 _initialCameraRotationDegrees;

  private float _maxX = 10.0f;
  private float _maxY = 10.0f;
  private float _maxZ = 5.0f;

  private float _noiseSpeed = 50.0f;

  private float _traumaReductionRate = 1.0f;
  private float _trauma = 0.0f;
  private float _time = 0.0f;

  public override void _Ready()
  {
    _noise = new OpenSimplexNoise();
    _random = new RandomNumberGenerator();
    _camera = GetViewport().GetCamera();
    _initialCameraRotationDegrees = GetViewport().GetCamera().RotationDegrees;
  }

  public override void _Process(float delta)
  {
    _time += delta;
    _trauma = Mathf.Max(_trauma - delta * _traumaReductionRate, 0.0f);

    _camera.RotationDegrees = new Vector3(
      _initialCameraRotationDegrees.x + _maxX * GetShakeIntensity() * GetNoiseFromSeed(0),
      _initialCameraRotationDegrees.y + _maxY * GetShakeIntensity() * GetNoiseFromSeed(1),
      _initialCameraRotationDegrees.z + _maxZ * GetShakeIntensity() * GetNoiseFromSeed(2)
    );
  }

  public void AddTrauma(float traumaAmount)
  {
    _trauma = Mathf.Clamp(_trauma + traumaAmount, 0.0f, 1.0f);
  }

  private float GetShakeIntensity()
  {
    return _trauma * _trauma;
  }

  private float GetNoiseFromSeed(int seed)
  {
    // _noise.Seed = seed;
    // return _noise.GetNoise1d(_time * _noiseSpeed);
    return _random.Randf();
  }
}
