//
// KinoGlitch - Video glitch effect
//
// Copyright (C) 2015 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DigitalGlitchEffect : MonoBehaviour
{
    public Shader shader;

    [SerializeField, Range(0, 1)] private float intensity;
    public float Intensity
    {
        get => intensity;
        set => intensity = Mathf.Clamp01(value);
    }

    private Material _material;
    private Texture2D _noiseTexture;
    private RenderTexture _trashFrame1;
    private RenderTexture _trashFrame2;

    private static Color RandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, Random.value);
    }

    private void SetUpResources()
    {
        if (_material != null) return;

        _material = new Material(shader);
        _material.hideFlags = HideFlags.DontSave;

        _noiseTexture = new Texture2D(64, 32, TextureFormat.ARGB32, false);
        _noiseTexture.hideFlags = HideFlags.DontSave;
        _noiseTexture.wrapMode = TextureWrapMode.Clamp;
        _noiseTexture.filterMode = FilterMode.Point;

        _trashFrame1 = new RenderTexture(Screen.width, Screen.height, 0);
        _trashFrame2 = new RenderTexture(Screen.width, Screen.height, 0);
        _trashFrame1.hideFlags = HideFlags.DontSave;
        _trashFrame2.hideFlags = HideFlags.DontSave;

        UpdateNoiseTexture();
    }

    private void UpdateNoiseTexture()
    {
        var color = RandomColor();

        for (var y = 0; y < _noiseTexture.height; y++)
        {
            for (var x = 0; x < _noiseTexture.width; x++)
            {
                if (Random.value > 0.89f) color = RandomColor();
                _noiseTexture.SetPixel(x, y, color);
            }
        }

        _noiseTexture.Apply();
    }

    private void Update()
    {
        if (Random.value > Mathf.Lerp(0.9f, 0.5f, Intensity))
        {
            SetUpResources();
            UpdateNoiseTexture();
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        SetUpResources();

        // Update trash frames on a constant interval.
        var fcount = Time.frameCount;
        if (fcount % 13 == 0) Graphics.Blit(source, _trashFrame1);
        if (fcount % 73 == 0) Graphics.Blit(source, _trashFrame2);

        _material.SetFloat("_Intensity", Intensity);
        _material.SetTexture("_NoiseTex", _noiseTexture);
        var trashFrame = Random.value > 0.5f ? _trashFrame1 : _trashFrame2;
        _material.SetTexture("_TrashTex", trashFrame);

        Graphics.Blit(source, destination, _material);
    }
}
