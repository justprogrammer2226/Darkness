using UnityEngine;
using UnityEngine.EventSystems;

public class LeverForMovingPlatform : Lever, IPointerDownHandler
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

        _actionWhenDeactivated = () =>
        {
            movingPlatform.isMoving = false;
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
            if((singleActivation && !_wasActivation) || !singleActivation || (IsActive && canDeactivate)) interactiveSprite.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            LeverInteraction leverInteraction = col.gameObject.GetComponent<LeverInteraction>();
            leverInteraction.currentLever = null;
            interactiveSprite.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(interactiveSprite.activeSelf)
        {
            if (IsActive)
            {
                Deactivate();
                if (singleActivation && _wasActivation)
                {
                    interactiveSprite.SetActive(false);
                }
            }
            else
            {
                Activate();
                if (!canDeactivate) interactiveSprite.SetActive(false);
            }
        }
    }
}
