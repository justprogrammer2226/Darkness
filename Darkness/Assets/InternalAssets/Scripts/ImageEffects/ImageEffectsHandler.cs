using System.Collections;
using UnityEngine;

public class ImageEffectsHandler : MonoBehaviour
{
    public static ImageEffectsHandler instance;

    #region Analog Glitch
    [Header("Analog Glitch")]
    [SerializeField] private Shader _analogGlitchShader;
    [SerializeField, Range(0, 1)] private float _analogScanLineJitter;
    [SerializeField, Range(0, 1)] private float _analogVerticalJump;
    [SerializeField, Range(0, 1)] private float _analogHorizontalShake;
    [SerializeField, Range(0, 1)] private float _analogColorDrift;
    public Shader AnalogGlitchShader
    {
        get => _analogGlitchEffect.shader;
        set => _analogGlitchEffect.shader = value;
    }
    public float AnalogScanLineJitter
    {
        get => _analogGlitchEffect.ScanLineJitter;
        set => _analogGlitchEffect.ScanLineJitter = Mathf.Clamp01(value);
    }
    public float AnalogVerticalJump
    {
        get => _analogGlitchEffect.VerticalJump;
        set => _analogGlitchEffect.VerticalJump = Mathf.Clamp01(value);
    }
    public float AnalogHorizontalShake
    {
        get => _analogGlitchEffect.HorizontalShake;
        set => _analogGlitchEffect.HorizontalShake = Mathf.Clamp01(value);
    }
    public float AnalogColorDrift
    {
        get => _analogGlitchEffect.ColorDrift;
        set => _analogGlitchEffect.ColorDrift = Mathf.Clamp01(value);
    }
    #endregion

    #region Digital Glitch
    [Header("Digital Glitch")]
    [SerializeField] private Shader _digitalGlitchShader;
    [SerializeField, Range(0, 1)] private float _digitalGlitchIntensity;
    public Shader DigitalGlitchShader
    {
        get => _digitalGlitchEffect.shader;
        set => _digitalGlitchEffect.shader = value;
    }
    public float DigitalGlitchIntensity
    {
        get => _digitalGlitchEffect.Intensity;
        set => _digitalGlitchEffect.Intensity = Mathf.Clamp01(value);
    }
    #endregion

    #region Glitch
    [Header("Glitch")]
    [SerializeField] private Shader _glitchShader;
    [SerializeField] private Texture2D _displacementMap;
    [SerializeField, Range(0, 1)] private float _glitchIntensity;
    [SerializeField, Range(0, 1)] private float _glitchFlipIntensity;
    [SerializeField, Range(0, 1)] private float _glitchColorIntensity;
    public Shader GlitchShader
    {
        get => _glitchEffect.shader;
        set => _glitchEffect.shader = value;
    }
    public Texture2D GlitchDisplacementMap
    {
        get => _glitchEffect.displacementMap;
        set => _glitchEffect.displacementMap = value;
    }
    public float GlitchIntensity
    {
        get => _glitchEffect.Intensity;
        set => _glitchEffect.Intensity = Mathf.Clamp01(value);
    }
    public float GlitchFlipIntensity
    {
        get => _glitchEffect.FlipIntensity;
        set => _glitchEffect.FlipIntensity = Mathf.Clamp01(value);
    }
    public float GlitchColorIntensity
    {
        get => _glitchEffect.ColorIntensity;
        set => _glitchEffect.ColorIntensity = Mathf.Clamp01(value);
    }
    #endregion

    #region Invert Color
    [Header("Invert color")]
    [SerializeField] private Shader invertColorShader;
    public Shader InvertColorShader
    {
        get => _invertColorEffect.shader;
        set => _invertColorEffect.shader = value;
    }
    #endregion

    private AnalogGlitchEffect _analogGlitchEffect;
    private DigitalGlitchEffect _digitalGlitchEffect;
    private GlitchEffect _glitchEffect;
    private InvertColorEffect _invertColorEffect;

    private void Start()
    {
        if(instance == null) instance = this;

        _analogGlitchEffect = gameObject.AddComponent<AnalogGlitchEffect>();
        AnalogGlitchShader = _analogGlitchShader;
        AnalogScanLineJitter = _analogScanLineJitter;
        AnalogVerticalJump = _analogVerticalJump;
        AnalogHorizontalShake = _analogHorizontalShake;
        AnalogColorDrift = _analogColorDrift;

        _digitalGlitchEffect = gameObject.AddComponent<DigitalGlitchEffect>();
        DigitalGlitchShader = _digitalGlitchShader;
        DigitalGlitchIntensity = _digitalGlitchIntensity;

        _glitchEffect = gameObject.AddComponent<GlitchEffect>();
        GlitchShader = _glitchShader;
        GlitchDisplacementMap = _displacementMap;
        GlitchIntensity = _glitchIntensity;
        GlitchFlipIntensity = _glitchFlipIntensity;
        GlitchColorIntensity = _glitchColorIntensity;

        _invertColorEffect = gameObject.AddComponent<InvertColorEffect>();
        InvertColorShader = invertColorShader;

        _invertColorEffect.enabled = false;
    }

    private void Update()
    {
        //AnalogGlitchShader = _analogGlitchShader;
        //AnalogScanLineJitter = _analogScanLineJitter;
        //AnalogVerticalJump = _analogVerticalJump;
        //AnalogHorizontalShake = _analogHorizontalShake;
        //AnalogColorDrift = _analogColorDrift;

        //DigitalGlitchShader = _digitalGlitchShader;
        //DigitalGlitchIntensity = _digitalGlitchIntensity;

        //GlitchShader = _glitchShader;
        //GlitchDisplacementMap = _displacementMap;
        //GlitchIntensity = _glitchIntensity;
        //GlitchFlipIntensity = _glitchFlipIntensity;
        //GlitchColorIntensity = _glitchColorIntensity;

        //InvertColorShader = invertColorShader;
    }

    public void AnalogGlitchEffectEnabled(bool enabled) => _analogGlitchEffect.enabled = enabled;

    public void DigitalGlitchEffectEnabled(bool enabled) =>_digitalGlitchEffect.enabled = enabled;

    public void GlitchEffectEnabled(bool enabled) => _glitchEffect.enabled = enabled;

    public void InvertColorEffectEnabled(bool enabled) => _invertColorEffect.enabled = enabled;

    public IEnumerator ActivateGlitch()
    {
        AnalogScanLineJitter = 1;

        while (AnalogScanLineJitter != 1)
        {
            AnalogScanLineJitter += 0.1f;
            yield return new WaitForSeconds(0.025f);
        }

        while (AnalogScanLineJitter != 0)
        {
            AnalogScanLineJitter -= 0.1f;
            yield return new WaitForSeconds(0.025f);
        }

        AnalogScanLineJitter = 0;
    }
}
