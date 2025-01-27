using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OreStacker : MonoBehaviour, IOreCounter
{
    private readonly Vector3 _initialPosition = new (0.375f, 1.33f, -0.375f);
    private readonly float _oreHorizontalSize = 0.25f;
    private readonly float _oreVerticalSize = 1.666667f;
    private readonly int _maxCountByX = 4;
    private readonly int _maxCountByZ = 4;

    private List<Ore> _ores = new();

    public event Action<Dictionary<string, int>> OresCountChanged;

    public Vector3 CalculatePosition(int countOresInStore)
    {
        Vector3 newPosition = _initialPosition;
        int squareByXZ = _maxCountByX * _maxCountByZ;

        int countByY = countOresInStore / squareByXZ;
        int countByZ = countOresInStore % squareByXZ / _maxCountByX;
        int countByX = countOresInStore % squareByXZ % _maxCountByX;

        newPosition.x -= countByX * _oreHorizontalSize;
        newPosition.z += countByZ * _oreHorizontalSize;
        newPosition.y += countByY * _oreVerticalSize;

        return newPosition;
    }

    public void Add(Ore ore)
    {
        ore.transform.localPosition = CalculatePosition(_ores.Count);
        _ores.Add(ore);

        OresCountChanged?.Invoke(GetCurrentOresCount());
    }

    public bool CheckOresForEnough(Dictionary<string, int> requireOres)
    {
        Dictionary<string, int> ores = GetCurrentOresCount();

        if (ores.Count == 0)
            return false;

        foreach (string key in requireOres.Keys)
        {
            if (ores.Keys.Contains(key) == false)
                return false;

            if (ores[key] < requireOres[key])
                return false;
        }

        return true;
    }

    public void Remove(KeyValuePair<string, int> removableOre)
    {
        var oresOfRequiredType = _ores.Where(ore => ore.Name == removableOre.Key).ToArray();

        for (int i = 0; i < removableOre.Value; i++)
        {
            _ores.Remove(oresOfRequiredType[i]);

            Destroy(oresOfRequiredType[i].gameObject);
        }

        OresCountChanged?.Invoke(GetCurrentOresCount());

        UpdatePositions();
    }

    private void UpdatePositions()
    {
        for (int i = 0; i < _ores.Count; i++)
            _ores[i].transform.localPosition = CalculatePosition(i);
    }

    private Dictionary<string, int> GetCurrentOresCount()
    {
        Dictionary<string, int> ores = new();
        int minOresCount = 1;

        foreach (Ore ore in _ores)
        {
            if (ores.ContainsKey(ore.Name))
                ores[ore.Name] += minOresCount;
            else
                ores.Add(ore.Name, minOresCount);
        }

        return ores;
    }
}
