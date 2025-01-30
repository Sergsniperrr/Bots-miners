using System;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Router : MonoBehaviour
{
    private Mover _mover;
    private Vector3 _basePosition;
    private Vector3 _waitingPoint;

    public event Action ArrivedToOre;
    public event Action ArrivedToUploadPoint;
    public event Action ArrivedToFlag;

    public void SetBasePosition(Vector3 position) => _basePosition = position;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
    }
    public void SetWaitingPoint(Vector3 position, bool isFree)
    {
        _waitingPoint = position;

        if (isFree)
            _mover.StartMoveTo(_waitingPoint);
    }

    public void GoToOre(Vector3 position)
    {
        _mover.StartMoveTo(position);

        _mover.ArrivedAtPoint += FinishMoveToOre;
    }

    public void GoToUploadPoint()
    {
        float indentForUpload = 1.8f;
        var direction = (_basePosition - transform.position).normalized;
        var point = _basePosition - direction * indentForUpload;

        _mover.ArrivedAtPoint -= FinishMoveToOre;

        _mover.StartMoveTo(point);

        _mover.ArrivedAtPoint += FinishMoveToUploadPoint;
    }

    public void GoToWaitingPoint()
    {
        _mover.ArrivedAtPoint -= FinishMoveToUploadPoint;

        _mover.StartMoveTo(_waitingPoint);
    }

    public void GoToFlag(Vector3 flagPosition)
    {
        float indentForUpload = 2.2f;
        var direction = (flagPosition - transform.position).normalized;
        var point = flagPosition - direction * indentForUpload;

        _mover.StartMoveTo(point);

        _mover.ArrivedAtPoint += FinishMoveToFlag;
    }

    private void FinishMoveToFlag()
    {
        _mover.ArrivedAtPoint -= FinishMoveToFlag;

        ArrivedToFlag?.Invoke();
    }

    private void FinishMoveToOre() => ArrivedToOre?.Invoke();
    private void FinishMoveToUploadPoint() => ArrivedToUploadPoint?.Invoke();
}