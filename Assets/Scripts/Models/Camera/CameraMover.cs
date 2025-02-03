using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _minPositionY;
    [SerializeField] private float _maxPositionY;

    private readonly PlayerInput _input = new();

    private Vector3 _direction = Vector3.zero;
    private Vector3 _rotation = Vector3.zero;
    private Vector3 _currentPosition;
    private float _rotatationSpeedMultiplier = 3.7f;

    private void Update()
    {
        Rotate();
        Move();
    }

    private void UpdateDirection()
    {
        _direction.x = _input.VelocityOfLeftRight;
        _direction.y = _input.VelocityOfUpDown;
        _direction.z = _input.VelocityOfForwardBackward;
    }

    private void Rotate()
    {
        _rotation.y = _input.VelocityOfHorizontalRotation;

        transform.Rotate(_speed * _rotatationSpeedMultiplier * Time.deltaTime * _rotation, Space.World);
    }

    private void Move()
    {
        UpdateDirection();

        transform.Translate(_speed * Time.deltaTime * _direction);

        _currentPosition = transform.position;
        _currentPosition.y = Mathf.Clamp(_currentPosition.y, _minPositionY, _maxPositionY);
        transform.position = _currentPosition;
    }
}
