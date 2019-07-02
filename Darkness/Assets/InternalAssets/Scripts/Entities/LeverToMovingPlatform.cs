using UnityEngine;

public class LeverToMovingPlatform : Lever
{
    public MovingPlatform movingPlatform;

    [Header("Debug")]
    [SerializeField] private bool _isActive;
    public override bool IsActive
    {
        get => _isActive;
        protected set => _isActive = value;
    }

    private void Start()
    {
        _actionWhenActivated = () =>
        {
            movingPlatform.isMoving = true;
        };
    }

    private void Update()
    {
        MoveLever();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            LeverInteraction leverInteraction = col.gameObject.GetComponent<LeverInteraction>();
            leverInteraction.currentLever = this;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            LeverInteraction leverInteraction = col.gameObject.GetComponent<LeverInteraction>();
            leverInteraction.currentLever = null;
        }
    }
}
