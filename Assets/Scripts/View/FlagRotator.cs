using System;
using UnityEngine;

public class FlagRotator : MonoBehaviour
{
    private Transform _camera;
    private Vector3 _rotation;

    private void Update()
    {
        transform.LookAt(_camera);

        _rotation = transform.rotation.eulerAngles;
        _rotation.x = 0f;
        _rotation.y += 90f;
        transform.rotation = Quaternion.Euler(_rotation);
    }

    public void InitializeCamera(Transform camera)
    {
        _camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));
    }
}
