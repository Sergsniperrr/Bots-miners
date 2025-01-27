using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MouseInputHandler))]
public class BaseHandler : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;

    private MouseInputHandler _input;
    private Collider _collider;
    private Base _selectedBase;
    private Flag _flag;

    private void Awake()
    {
        _input = GetComponent<MouseInputHandler>();
    }

    private void Update()
    {
        HandleClickedObjects();
    }

    private void HandleClickedObjects()
    {
        _collider = _input.OnLeftClick();

        if (_collider == null)
            return;

        if (_collider.gameObject.TryGetComponent(out Base selectedBase))
        {
            _selectedBase = selectedBase;
            _selectedBase.ShowAura();

            if (_flag == null)
                _flag = Instantiate(_flagPrefab, _selectedBase.transform.position, transform.rotation);
        }
        else
        {
            if (_selectedBase != null)
                _selectedBase.HideAura();

            if (_flag != null)
                Destroy(_flag.gameObject);
        }
    }
}
