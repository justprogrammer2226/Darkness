using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float scale;

    public void OnPointerDown(PointerEventData data)
    {
        transform.localScale = new Vector3(scale, scale, scale);
    }

    public void OnPointerUp(PointerEventData data)
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
