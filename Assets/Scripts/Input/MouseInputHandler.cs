using System;
using UnityEngine;

public class MouseInputHandler : MonoBehaviour
{
    [SerializeField] private Collider _platform;
    [SerializeField] private Camera _mainCamera;

    private readonly float _positionOnY = 0.13f;
    private readonly PlayerInput _input = new();

    private Ray _ray;
    private RaycastHit _hitInfo;
    private RaycastHit _hit;
    private Vector3 _collisionPoint;
    private Collider _collider;

    public event Action<Base> BaseSelected;
    public event Action SelectionCancelled;

    private void Update()
    {
        HandleLeftClick();
        HandleRightClick();
    }

    public Vector3 GetMousePosition()
    {
        _ray = _mainCamera.ScreenPointToRay(_input.MousePosition);

        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
        {
            if (_hit.collider == _platform)
            {
                _collisionPoint = _hit.point;
                _collisionPoint.y = _positionOnY;
            }
        }

        return _collisionPoint;
    }

    private void HandleRightClick()
    {
        if (_input.OnRightClick)
            SelectionCancelled?.Invoke();
    }

    private void HandleLeftClick()
    {
        _collider = OnMouseClick(_input.OnLeftClick);

        if (_collider == null)
            return;

        if (_collider.gameObject.TryGetComponent(out Base targetBase))
            BaseSelected?.Invoke(targetBase);
        else if (_collider.gameObject.TryGetComponent(out Platform _))
            BaseSelected?.Invoke(null);
    }

    private Collider OnMouseClick(bool isClick)
    {
        if (isClick)
        {
            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hitInfo))
                return _hitInfo.collider;
        }

        return null;
    }
}
