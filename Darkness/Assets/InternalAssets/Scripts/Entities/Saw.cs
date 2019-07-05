using UnityEngine;

public class Saw : MonoBehaviour
{
    public Vector3 eulers;
    public bool isRotated;

    private void Update()
    {
        if(isRotated) transform.Rotate(eulers);
    }
}
