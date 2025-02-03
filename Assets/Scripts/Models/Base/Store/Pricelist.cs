using System.Collections.Generic;
using System;
using UnityEngine;

public class Pricelist : MonoBehaviour
{
    [SerializeField] private string[] _oresForMiner;
    [SerializeField] private string[] _oresForBase;

    public Dictionary<string, int> MinerPrice { get; private set; }
    public Dictionary<string, int> BasePrice { get; private set; }

    private void Awake()
    {
        if (_oresForMiner.Length == 0)
            throw new NullReferenceException(nameof(_oresForMiner));

        if (_oresForBase.Length == 0)
            throw new NullReferenceException(nameof(_oresForBase));

        MinerPrice = CreatePrice(_oresForMiner);
        BasePrice = CreatePrice(_oresForBase);
    }

    private Dictionary<string, int> CreatePrice(string[] ores)
    {
        Dictionary<string, int> price = new();
        int minCount = 1;

        foreach (string ore in ores)
        {
            if (price.ContainsKey(ore))
                price[ore] += minCount;
            else
                price.Add(ore, minCount);
        }

        return price;
    }
}
