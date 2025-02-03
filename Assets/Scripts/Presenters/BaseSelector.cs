using System;
using UnityEngine;

[RequireComponent(typeof(MouseInputHandler))]
[RequireComponent(typeof(SelectedBaseView))]
[RequireComponent(typeof(FlagPositionChecker))]
public class BaseSelector : MonoBehaviour
{
    private Base _selectedBase;
    private MouseInputHandler _input;
    private SelectedBaseView _view;
    private FlagPositionChecker _flagPositionChecker;
    private Vector3 _flagPosition;

    public event Action<Base> BaseSelected;

    private void Awake()
    {
        _input = GetComponent<MouseInputHandler>();
        _view = GetComponent<SelectedBaseView>();
        _flagPositionChecker = GetComponent<FlagPositionChecker>();
    }

    private void OnEnable()
    {
        _input.BaseSelected += SelectBase;
        _input.SelectionCancelled += CancelSelection;
    }

    private void OnDisable()
    {
        _input.BaseSelected -= SelectBase;
        _input.SelectionCancelled -= CancelSelection;
    }

    private void SelectBase(Base targetBase)
    {
        if (targetBase == null)
        {
            if (_selectedBase != null)
                SetFlagPosition();

            return;
        }

        if (targetBase == _selectedBase)
        {
            _selectedBase.ResetFlagPosition();
            _view.ShowSelectedBase(_selectedBase);
        }
        else
        {
            if (_selectedBase != null)
                _view.UnselectBase(_selectedBase);

            _selectedBase = targetBase;
            _view.ShowSelectedBase(_selectedBase);
            BaseSelected?.Invoke(_selectedBase);
        }
    }

    private void SetFlagPosition()
    {
        _flagPosition = _input.GetMousePosition();

        if (_flagPositionChecker.CheckForNearbyBase(_flagPosition))
            return;

        _selectedBase.SetFlagPosition(_flagPosition);
        _view.ShowSelectedBase(_selectedBase);
    }

    private void CancelSelection()
    {
        if (_selectedBase != null)
            _view.UnselectBase(_selectedBase);

        _selectedBase = null;

        BaseSelected?.Invoke(null);
    }
}
