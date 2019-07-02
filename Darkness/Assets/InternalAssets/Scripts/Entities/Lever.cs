using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Lever : MonoBehaviour
{
    [Header("Settings")]
    public GameObject stick;

    public bool singleActivation;
    public bool canDeactivate;
    public float speed;

    public Vector3 activateAngle;
    public Vector3 deactivateAngle;

    public abstract bool IsActive { get; protected set; }

    protected bool _wasActivation;
    protected Action _actionWhenActivated, _actionWhenDeactivated;
    protected Quaternion _targetAngle;

    public void Activate()
    {
        if((singleActivation && !_wasActivation) || !singleActivation)
        {
            IsActive = true;
            _actionWhenActivated?.Invoke();
            _targetAngle = Quaternion.Euler(activateAngle);
            _wasActivation = true;
        }
    }

    public void Deactivate()
    {
        if(canDeactivate)
        {
            IsActive = false;
            _actionWhenDeactivated?.Invoke();
            _targetAngle = Quaternion.Euler(deactivateAngle);
        }
    }

    public void MoveLever()
    {
        stick.transform.rotation = Quaternion.RotateTowards(stick.transform.rotation, _targetAngle, speed * Time.deltaTime);
    }
}
