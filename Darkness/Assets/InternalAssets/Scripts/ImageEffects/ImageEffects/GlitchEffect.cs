/**
This work is licensed under a Creative Commons Attribution 3.0 Unported License.
http://creativecommons.org/licenses/by/3.0/deed.en_GB

You are free:

to copy, distribute, display, and perform the work
to make derivative works
to make commercial use of the work
*/

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GlitchEffect : MonoBehaviour
{
	public Texture2D displacementMap;
	public Shader shader;

	[Header("Glitch Intensity")]
	[SerializeField, Range(0, 1)] private float _intensity;
    public float Intensity
    {
        get => _intensity;
        set => _intensity = Mathf.Clamp01(value);
    }

    [SerializeField, Range(0, 1)] private float _flipIntensity;
    public float FlipIntensity
    {
        get => _flipIntensity;
        set => _flipIntensity = Mathf.Clamp01(value);
    }

    [SerializeField, Range(0, 1)] private float _colorIntensity;
    public float ColorIntensity
    {
        get => _colorIntensity;
        set => _colorIntensity = Mathf.Clamp01(value);
    }


    private float _glitchup;
	private float _glitchdown;
	private float flicker;
	private float _glitchupTime = 0.05f;
	private float _glitchdownTime = 0.05f;
	private float _flickerTime = 0.5f;
	private Material _material;

	private void Start()
	{
		_material = new Material(shader);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		_material.SetFloat("_Intensity", Intensity);
		_material.SetFloat("_ColorIntensity", ColorIntensity);
		_material.SetTexture("_DispTex", displacementMap);

		flicker += Time.deltaTime * ColorIntensity;
		if (flicker > _flickerTime)
		{
			_material.SetFloat("filterRadius", Random.Range(-3f, 3f) * ColorIntensity);
			_material.SetVector("direction", Quaternion.AngleAxis(Random.Range(0, 360) * ColorIntensity, Vector3.forward) * Vector4.one);
			flicker = 0;
			_flickerTime = Random.value;
		}

		if (ColorIntensity == 0)
			_material.SetFloat("filterRadius", 0);

		_glitchup += Time.deltaTime * FlipIntensity;
		if (_glitchup > _glitchupTime)
		{
			if (Random.value < 0.1f * FlipIntensity)
				_material.SetFloat("flip_up", Random.Range(0, 1f) * FlipIntensity);
			else
				_material.SetFloat("flip_up", 0);

			_glitchup = 0;
			_glitchupTime = Random.value / 10f;
		}

		if (FlipIntensity == 0)
			_material.SetFloat("flip_up", 0);

		_glitchdown += Time.deltaTime * FlipIntensity;
		if (_glitchdown > _glitchdownTime)
		{
			if (Random.value < 0.1f * FlipIntensity)
				_material.SetFloat("flip_down", 1 - Random.Range(0, 1f) * FlipIntensity);
			else
				_material.SetFloat("flip_down", 1);

			_glitchdown = 0;
			_glitchdownTime = Random.value / 10f;
		}

		if (FlipIntensity == 0)
			_material.SetFloat("flip_down", 1);

		if (Random.value < 0.05 * Intensity)
		{
			_material.SetFloat("displace", Random.value * Intensity);
			_material.SetFloat("scale", 1 - Random.value * Intensity);
		}
		else
			_material.SetFloat("displace", 0);

		Graphics.Blit(source, destination, _material);
	}
}
