using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OreDetector : MonoBehaviour
{
    [SerializeField] private float _radius = 15f;

    private float _searchDelay = 0.5f;
    private float _waitSearchCounter;
    private List<Ore> _ores = new();

    public event Action<Ore> OreDetected;

    private void Update()
    {
        _waitSearchCounter += Time.deltaTime;

        if (_waitSearchCounter >= _searchDelay)
        {
            Scan();
            _waitSearchCounter = 0f;
        }
    }

    private void Scan()
    {
        Vector3 scanSize = new(_radius, 0.01f, _radius);
        Vector3 shift = new(0f, 0.2f, 0f);

        Collider[] hitColliders = Physics.OverlapBox(transform.position + shift, scanSize, Quaternion.identity);

        SearchNearestOre(GetEnabledOres(hitColliders));
    }

    private Ore[] GetEnabledOres(Collider[] colliders)
    {
        _ores.Clear();

        foreach (Collider collider in colliders)
            if (collider.gameObject.TryGetComponent(out Ore ore) && ore.IsEnable)
                _ores.Add(ore);

        return _ores.ToArray();
    }

    private void SearchNearestOre(Ore[] ores)
    {
        Ore ore = ores.OrderBy(ore => Vector3.SqrMagnitude(transform.position - ore.transform.position)).FirstOrDefault();

        if (ore != null)
            OreDetected?.Invoke(ore);
    }
}
