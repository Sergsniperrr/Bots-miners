using System;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private Transform _camera;
    private Vector3 _rotation;
    private float _rotationOnX = 0f;
    private float _rotationOnY = 90f;

    private void Update()
    {
        transform.LookAt(_camera);

        _rotation = transform.rotation.eulerAngles;
        _rotation.x = _rotationOnX;
        _rotation.y += _rotationOnY;
        transform.rotation = Quaternion.Euler(_rotation);
    }

    public void InitializeCamera(Transform camera)
    {
        _camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));
    }
}
