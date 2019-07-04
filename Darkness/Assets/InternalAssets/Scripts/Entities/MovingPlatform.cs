using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform firstPosition, secondPosition;
    public float smoothTime;
    public bool isMoving = true;

    private Vector3 _velocity = Vector3.zero;
    private Vector3 _targetPosition, _currentPosition;

    private void Start()
    {
        float distance1 = Vector3.Distance(firstPosition.position, transform.position);
        float distance2 = Vector3.Distance(secondPosition.position, transform.position);

        if(distance1 < distance2)
        {
            _targetPosition = firstPosition.position;
        }
        else
        {
            _targetPosition = secondPosition.position;
        }
    }

    private void Update()
    {
        if(isMoving)
        {
            CalculateTargetPosition();
            _currentPosition = Vector3.MoveTowards(transform.position, _targetPosition, smoothTime * Time.deltaTime);
            transform.position = _currentPosition;
        }
    }

    private void CalculateTargetPosition()
    {
        if (_currentPosition == firstPosition.position) _targetPosition = secondPosition.position;
        else if (_currentPosition == secondPosition.position) _targetPosition = firstPosition.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(firstPosition.position, secondPosition.position);
    }
}
