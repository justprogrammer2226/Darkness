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
public class AnalogGlitchEffect : MonoBehaviour
{
    public Shader shader;

    [SerializeField, Range(0, 1)] private float scanLineJitter;
    public float ScanLineJitter
    {
        get => scanLineJitter;
        set => scanLineJitter = Mathf.Clamp01(value);
    }

    [SerializeField, Range(0, 1)] private float verticalJump;
    public float VerticalJump
    {
        get => verticalJump; 
        set => verticalJump = Mathf.Clamp01(value);
    }

    [SerializeField, Range(0, 1)] private float horizontalShake;
    public float HorizontalShake
    {
        get => horizontalShake;
        set => horizontalShake = Mathf.Clamp01(value);
    }

    [SerializeField, Range(0, 1)] private float colorDrift;
    public float ColorDrift
    {
        get => colorDrift;
        set => colorDrift = Mathf.Clamp01(value);
    }

    private Material _material;
    private float _verticalJumpTime;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_material == null)
        {
            _material = new Material(shader);
            _material.hideFlags = HideFlags.DontSave;
        }

        _verticalJumpTime += Time.deltaTime * VerticalJump * 11.3f;

        var sl_thresh = Mathf.Clamp01(1.0f - ScanLineJitter * 1.2f);
        var sl_disp = 0.002f + Mathf.Pow(ScanLineJitter, 3) * 0.05f;
        _material.SetVector("_ScanLineJitter", new Vector2(sl_disp, sl_thresh));

        var vj = new Vector2(VerticalJump, _verticalJumpTime);
        _material.SetVector("_VerticalJump", vj);

        _material.SetFloat("_HorizontalShake", HorizontalShake * 0.2f);

        var cd = new Vector2(ColorDrift * 0.04f, Time.time * 606.11f);
        _material.SetVector("_ColorDrift", cd);

        Graphics.Blit(source, destination, _material);
    }
}