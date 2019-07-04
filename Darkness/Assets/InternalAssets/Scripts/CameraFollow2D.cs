using UnityEngine;

public enum HorizontalMovement
{
    Left,
    Right
}

public class CameraFollow2D : MonoBehaviour
{
    [Header("Settings")]
    public Transform player;
    public float smoothTime;
    public Vector2 offset;

    [Header("Debug")]
    public HorizontalMovement horizontalMovement;

    private float _currentX, _lastX;
    private GameObject _camera;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _targetPosition, _currentPosition;

    private void Start()
    {
        offset.Set(Mathf.Abs(offset.x), offset.y);
        _lastX = player.position.x;
        _camera = gameObject;
    }    

    private void Update()
    {
        ControlCamera();
    }

    /// <summary> Control camera. </summary>
    private void ControlCamera()
    {
        CalculateHorizontalMovementDirection(ref horizontalMovement);
        _targetPosition.Set(horizontalMovement == HorizontalMovement.Left ? player.position.x - offset.x : player.position.x + offset.x, player.position.y + offset.y, _camera.transform.position.z);
        _currentPosition = Vector3.SmoothDamp(_camera.transform.position, _targetPosition, ref _velocity, smoothTime * Time.deltaTime);
        _camera.transform.position = _currentPosition;
    }

    /// <summary> Calculate horizontal movement direction by player movement. </summary>
    private void CalculateHorizontalMovementDirection(ref HorizontalMovement currentHorizontalMovement)
    {
        _currentX = player.position.x;
        if (_currentX > _lastX && Mathf.Abs(_currentX - _lastX) > 0.0001) currentHorizontalMovement = HorizontalMovement.Right;
        else if(_currentX < _lastX && Mathf.Abs(_currentX - _lastX) > 0.0001) currentHorizontalMovement = HorizontalMovement.Left;
        _lastX = player.position.x;
    }
}