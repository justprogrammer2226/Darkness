using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingPlatform : MonoBehaviour
{
    [Tooltip("After what time the platform will fall?")]
    public float dropTime;
    [Tooltip("Do I need to destroy the platform after it falls?")]
    public bool destroyPlatform;
    [Tooltip("How long does it take to destroy the platform if it fell?")]
    public float destroyTime;

    private Rigidbody2D _rb2d;

    private void Start()
    {
        _rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            Invoke("DropPlatform", dropTime);
            if(destroyPlatform) Destroy(gameObject, destroyTime);
        }
    }

    private void DropPlatform()
    {
        _rb2d.isKinematic = false;
    }
}
