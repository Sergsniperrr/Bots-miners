using System;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Vector3 _target;
    private Vector3 _direction;
    private Vector3 _newPosition;
    private float _sqrDistance;

    public event Action ArrivedAtPoint;

    private void Update()
    {
        if (_target != Vector3.zero)
            Move();
    }

    public void StartMoveTo(Vector3 target) =>
        _target = target;

    private void Move()
    {
        _direction = _target - transform.position;
        _direction.y = 0f;
        _sqrDistance = _direction.sqrMagnitude;
        _direction.Normalize();

        if (_sqrDistance < 0.5f)
        {
            _direction = Vector3.zero;

            _target = Vector3.zero;
            ArrivedAtPoint?.Invoke();
        }

        _newPosition = transform.position + _speed * Time.deltaTime * _direction;
        transform.position = _newPosition;
    }
}
