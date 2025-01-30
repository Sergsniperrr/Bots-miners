using System;
using UnityEngine;

public class MinerSpawner : MonoBehaviour
{
    private Miner _minerPrefab;

    public void InitializeData(Miner minerPrefab)
    {
        _minerPrefab = minerPrefab != null ? minerPrefab : throw new ArgumentNullException(nameof(minerPrefab));
    }

    public Miner CreateMiner()
    {
        Vector3 position = transform.position;
        float offsetY = 0.87f;
        position.y = offsetY;

        return Instantiate(_minerPrefab, position, _minerPrefab.transform.rotation);
    }
}
