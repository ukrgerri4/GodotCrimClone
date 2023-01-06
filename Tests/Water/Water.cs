using System.Collections.Generic;
using Godot;

// [Tool]
public class Water : ImmediateGeometry
{
  private class ImgDetails
  {
    public float Amplitude;
    public float Steepness;
    public float WindDirectionX;
    public float WindDirectionY;
    public float Frequency;
  }

  private RandomNumberGenerator _random;
  public const int NUMBER_OF_WAVES = 10;
  private bool _initialized = false;
  private int res = 100;
  private float counter = 0.5f;
  private List<ImgDetails> waves = new List<ImgDetails>();
  private ImageTexture waves_in_tex = new ImageTexture();

  private float waveLength = 60;
  [Export(PropertyHint.Range, "0, 10000, 1")]
  public float WaveLength
  {
    get => waveLength;
    set
    {
      waveLength = value;
      UpdateWavesIfInitialized();
    }
  }

  private float steepness = 0.01f;
  [Export(PropertyHint.Range, "0, 1, 0.01")]
  public float Steepness
  {
    get => steepness;
    set
    {
      steepness = value;
      UpdateWavesIfInitialized();
    }
  }

  private float amplitude = 0.25f;
  [Export(PropertyHint.Range, "0, 10000, 1")]
  public float Amplitude
  {
    get => amplitude;
    set
    {
      amplitude = value;
      UpdateWavesIfInitialized();
    }
  }

  private Vector2 windDirection = new Vector2(1, 0);
  [Export]
  public Vector2 WindDirection
  {
    get => windDirection;
    set
    {
      windDirection = value;
      UpdateWavesIfInitialized();
    }
  }

  private float windAlign = 0.0f;
  [Export(PropertyHint.Range, "0, 1, 0.01")]
  public Vector2 WindAlign
  {
    get => windDirection;
    set
    {
      windDirection = value;
      UpdateWavesIfInitialized();
    }
  }

  private float speed = 10.0f;
  [Export(PropertyHint.Range, "0, 100, 0.1")]
  public float Speed
  {
    get => speed;
    set
    {
      speed = value;
      MaterialOverride.Set("shader_param/speed", speed);
    }
  }

  private bool noiseEnabled = true;
  [Export(PropertyHint.Flags, "false, true")]
  public bool NoiseEnabled
  {
    get => noiseEnabled;
    set
    {
      noiseEnabled = value;
      var noiseParams = (Vector4)MaterialOverride.Get("shader_param/noise_params");
      noiseParams.d = noiseEnabled ? 1 : 0;
      MaterialOverride.Set("shader_param/noise_params", noiseParams);
    }
  }

  private float noiseAmplitude = 0.28f;
  [Export(PropertyHint.Range, "0, 1, 0.01")]
  public float NoiseAmplitude
  {
    get => noiseAmplitude;
    set
    {
      noiseAmplitude = value;
      var noiseParams = (Vector4)MaterialOverride.Get("shader_param/noise_params");
      noiseParams.x = noiseAmplitude;
      MaterialOverride.Set("shader_param/noise_params", noiseParams);
    }
  }

  private float noiseFrequency = 0.065f;
  [Export(PropertyHint.Range, "0, 1, 0.01")]
  public float NoiseFrequency
  {
    get => noiseFrequency;
    set
    {
      noiseFrequency = value;
      var noiseParams = (Vector4)MaterialOverride.Get("shader_param/noise_params");
      noiseParams.y = noiseFrequency;
      MaterialOverride.Set("shader_param/noise_params", noiseParams);
    }
  }

  private float noiseSpeed = 0.48f;
  [Export(PropertyHint.Range, "0, 1, 0.01")]
  public float NoiseSpeed
  {
    get => noiseSpeed;
    set
    {
      noiseSpeed = value;
      var noiseParams = (Vector4)MaterialOverride.Get("shader_param/noise_params");
      noiseParams.z = noiseSpeed;
      MaterialOverride.Set("shader_param/noise_params", noiseParams);
    }
  }

  private uint seedValue = 0;

  [Export(PropertyHint.Range, "0, 100, 1")]
  public uint SeedValue
  {
    get => seedValue;
    set
    {
      seedValue = value;
      UpdateWavesIfInitialized();
    }
  }

  public override void _Ready()
  {
    _random = new RandomNumberGenerator();
    _random.Randomize();

    for (int i = 0; i < NUMBER_OF_WAVES; i++)
    {
      waves.Add(new ImgDetails());
    }

    for (int j = 0; j < res; j++)
    {
      var y = (float)(j / res - 0.5);
      var n_y = (float)((j + 1) / res - 0.5);
      Begin(Mesh.PrimitiveType.TriangleStrip);
      for (int i = 0; i < res; i++)
      {
        var x = (float)(i / res - 0.5);
        AddVertex(new Vector3(x * 2, 0, -y * 2));
        AddVertex(new Vector3(x * 2, 0, -n_y * 2));
      }
      End();
    }
    Begin(Mesh.PrimitiveType.Points);
    AddVertex(-1 * new Vector3(1, 1, 1) * Mathf.Pow(2, 32));
    AddVertex(new Vector3(1, 1, 1) * Mathf.Pow(2, 32));
    End();
    UpdateWaves();
  }

  public override void _Process(float delta)
  {
    MaterialOverride.Set("shader_param/time_offset", OS.GetTicksMsec() / 1000.0 * Speed);
    _initialized = true;
  }

  private void UpdateWavesIfInitialized()
  {
    if (_initialized)
    {
      UpdateWaves();
    }
  }

  private void UpdateWaves()
  {
    _random.Seed = seedValue;
    var amp_length_ratio = amplitude / waveLength;
    for (int i = 0; i < NUMBER_OF_WAVES; i++)
    {
        var _wavelength = _random.RandfRange((float)(waveLength/2.0), (float)(waveLength*2.0));
        var _wind_direction = windDirection.Rotated(_random.RandfRange(-Mathf.Pi, Mathf.Pi)*(1-windAlign));
        ((ImgDetails)waves[i]).Amplitude = amp_length_ratio * _wavelength;
        ((ImgDetails)waves[i]).Steepness = _random.RandfRange(0, steepness);
        ((ImgDetails)waves[i]).WindDirectionX = _wind_direction.x;
        ((ImgDetails)waves[i]).WindDirectionY = _wind_direction.y;
        ((ImgDetails)waves[i]).Frequency = Mathf.Sqrt((float)(0.098 * Mathf.Tau / _wavelength));
    }

    var img = new Image();
    img.Create(5, NUMBER_OF_WAVES, false, Image.Format.Rf);
    img.Lock();
    for (int i = 0; i < NUMBER_OF_WAVES; i++)
    {
      img.SetPixel(0, i, new Color(((ImgDetails)waves[i]).Amplitude, 0, 0, 0));
      img.SetPixel(1, i, new Color(((ImgDetails)waves[i]).Steepness, 0, 0, 0));
      img.SetPixel(2, i, new Color(((ImgDetails)waves[i]).WindDirectionX, 0, 0, 0));
      img.SetPixel(3, i, new Color(((ImgDetails)waves[i]).WindDirectionY, 0, 0, 0));
      img.SetPixel(4, i, new Color(((ImgDetails)waves[i]).Frequency, 0, 0, 0));
    }
    img.Unlock();
    waves_in_tex.CreateFromImage(img, 0);
    MaterialOverride.Set("shader_param/waves", waves_in_tex);
  }
}

/*
#tool
extends ImmediateGeometry

const NUMBER_OF_WAVES = 10;

export(float, 0, 10000) var wavelength = 60.0 setget set_wavelength
export(float, 0, 1) var steepness = 0.01 setget set_steepness
export(float, 0, 10000) var amplitude = 0.25 setget set_amplitude
export(Vector2) var wind_direction = Vector2(1, 0) setget set_wind_direction
export(float, 0, 1) var wind_align = 0.0 setget set_wind_align
export(float) var speed = 10.0 setget set_speed

export(bool) var noise_enabled = true setget set_noise_enabled
export(float) var noise_amplitude = 0.28 setget set_noise_amplitude
export(float) var noise_frequency = 0.065 setget set_noise_frequency
export(float) var noise_speed = 0.48 setget set_noise_speed

export(int) var seed_value = 0 setget set_seed

var res = 100.0
var initialized = false

var counter = 0.5
#var cube_cam = preload("res://CubeCamera.tscn")
#var cube_cam_inst;

var waves = []
var waves_in_tex = ImageTexture.new()

func _ready():
	
	for j in range(res):
		var y = j/res - 0.5
		var n_y = (j+1)/res - 0.5
		begin(Mesh.PRIMITIVE_TRIANGLE_STRIP)
		for i in range(res):
			var x = i/res - 0.5
			
			add_vertex(Vector3(x*2, 0, -y*2))
			
			add_vertex(Vector3(x*2, 0, -n_y*2))
		end()
	begin(Mesh.PRIMITIVE_POINTS)
	add_vertex(-Vector3(1,1,1)*pow(2,32))
	add_vertex(Vector3(1,1,1)*pow(2,32))
	end()
	
	waves_in_tex = ImageTexture.new()
	update_waves()
	
#	cube_cam_inst = cube_cam.instance()
#	add_child(cube_cam_inst)


func _process(delta):
	counter -= delta
	if counter <= 0:
#		var cube_map = cube_cam_inst.update_cube_map()
#		material_override.set_shader_param('environment', cube_map)
		counter = INF
	
	material_override.set_shader_param('time_offset', OS.get_ticks_msec()/1000.0 * speed)
	initialized = true

func set_wavelength(value):
	wavelength = value
	if initialized:
		update_waves()

func set_steepness(value):
	steepness = value
	if initialized:
		update_waves()

func set_amplitude(value):
	amplitude = value
	if initialized:
		update_waves()

func set_wind_direction(value):
	wind_direction = value
	if initialized:
		update_waves()

func set_wind_align(value):
	wind_align = value
	if initialized:
		update_waves()

func set_seed(value):
	seed_value = value
	if initialized:
		update_waves()

func set_speed(value):
	speed = value
	material_override.set_shader_param('speed', value)

func set_noise_enabled(value):
	noise_enabled = value
	var old_noise_params = material_override.get_shader_param('noise_params')
	old_noise_params.d = 1 if value else 0
	material_override.set_shader_param('noise_params', old_noise_params)

func set_noise_amplitude(value):
	noise_amplitude = value
	var old_noise_params = material_override.get_shader_param('noise_params')
	old_noise_params.x = value
	material_override.set_shader_param('noise_params', old_noise_params)

func set_noise_frequency(value):
	noise_frequency = value
	var old_noise_params = material_override.get_shader_param('noise_params')
	old_noise_params.y = value
	material_override.set_shader_param('noise_params', old_noise_params)

func set_noise_speed(value):
	noise_speed = value
	var old_noise_params = material_override.get_shader_param('noise_params')
	old_noise_params.z = value
	material_override.set_shader_param('noise_params', old_noise_params)

func get_displace(position):
	
	var new_p;
	if typeof(position) == TYPE_VECTOR3:
		new_p = Vector3(position.x, 0.0, position.z)
	elif typeof(position) == TYPE_VECTOR2:
		new_p = Vector3(position.x, 0.0, position.y)
	else:
		printerr('Position is not a vector3!')
		breakpoint
	
	var w; var amp; var steep; var phase; var dir
	for i in waves:
		amp = i['amplitude']
		if amp == 0.0: continue
		
		dir = Vector2(i['wind_directionX'], i['wind_directionY'])
		w = i['frequency']
		steep = i['steepness'] / (w*amp)
		phase = 2.0 * w
		
		var W = position.dot(w*dir) + phase * OS.get_ticks_msec()/1000.0 * speed
		
		new_p.x += steep*amp * dir.x * cos(W)
		new_p.z += steep*amp * dir.y * cos(W)
		new_p.y += amp * sin(W)
	return new_p;

func update_waves():
	#Generate Waves..
	seed(seed_value)
	var amp_length_ratio = amplitude / wavelength
	waves.clear()
	for _i in range(NUMBER_OF_WAVES):
		var _wavelength = rand_range(wavelength/2.0, wavelength*2.0)
		var _wind_direction = wind_direction.rotated(rand_range(-PI, PI)*(1-wind_align))
		
		waves.append({
			'amplitude': amp_length_ratio * _wavelength,
			'steepness': rand_range(0, steepness),
			'wind_directionX': _wind_direction.x,
			'wind_directionY': _wind_direction.y,
			'frequency': sqrt(0.098 * TAU/_wavelength)
		})
	#Put Waves in Texture..
	var img = Image.new()
	img.create(5, NUMBER_OF_WAVES, false, Image.FORMAT_RF)
	img.lock()
	for i in range(NUMBER_OF_WAVES):
		img.set_pixel(0, i, Color(waves[i]['amplitude'], 0,0,0))
		img.set_pixel(1, i, Color(waves[i]['steepness'], 0,0,0))
		img.set_pixel(2, i, Color(waves[i]['wind_directionX'], 0,0,0))
		img.set_pixel(3, i, Color(waves[i]['wind_directionY'], 0,0,0))
		img.set_pixel(4, i, Color(waves[i]['frequency'], 0,0,0))
	img.unlock()
	waves_in_tex.create_from_image(img, 0)
	
	material_override.set_shader_param('waves', waves_in_tex)

*/