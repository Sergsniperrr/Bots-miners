using System;
using UnityEngine;

[RequireComponent(typeof(MouseInput))]
[RequireComponent(typeof(SelectedBaseView))]
public class BaseSelecter : MonoBehaviour
{
    private Base _selectedBase;
    private Collider _collider;
    private MouseInput _input;
    private SelectedBaseView _view;

    public event Action<Base> BaseSelected;

    private void Awake()
    {
        _input = GetComponent<MouseInput>();
        _view = GetComponent<SelectedBaseView>();
    }

    private void Update()
    {
        HandleLeftClick();
        HandleRightClick();
    }

    private void SelectBase(Base targetBase)
    {
        if (targetBase == _selectedBase)
        {
            _selectedBase.ResetFlagPosition();
            _view.ShowSelectedBase(_selectedBase);
        }
        else
        {
            if (_selectedBase != null)
                _selectedBase.HideAura();

            _selectedBase = targetBase;
            _view.ShowSelectedBase(_selectedBase);
            BaseSelected?.Invoke(_selectedBase);
        }
    }

    private void SetFlagPosition(Vector3 position)
    {
        _selectedBase.SetFlagPosition(position);
        _view.ShowSelectedBase(_selectedBase);
    }

    private void Unselect(Base targetBase)
    {
        if (targetBase == null)
            return;

        _view.UnselectBase(targetBase);
    }

    private void HandleLeftClick()
    {
        _collider = _input.OnLeftClick();

        if (_collider == null)
            return;

        if (_collider.gameObject.TryGetComponent(out Base targetBase))
        {
            SelectBase(targetBase);

            BaseSelected?.Invoke(_selectedBase);
        }

        if (_collider.gameObject.TryGetComponent(out Platform _) && _selectedBase != null)
            SetFlagPosition(_input.OnMouseMove());
    }

    private void HandleRightClick()
    {
        if (_input.OnRightClick())
        {
            Unselect(_selectedBase);
            _selectedBase = null;

            BaseSelected?.Invoke(null);
        }
    }
}
