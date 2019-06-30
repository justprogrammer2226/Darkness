using UnityEngine;

[RequireComponent(typeof(Camera))]
public class InvertColorEffect : MonoBehaviour
{
	public Shader shader;
	private Material _material;

	private void Start()
	{
		_material = new Material(shader);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, _material);
	}
}
