using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pricelist : MonoBehaviour
{
    [SerializeField] private string[] _oresForMiner;
    [SerializeField] private string[] _oresForBase;

    public Dictionary<string, int> MinerPrice { get; private set; }
    public Dictionary<string, int> BasePrice { get; private set; }

    private void Awake()
    {
        MinerPrice = CreatePrice(_oresForMiner);
        BasePrice = CreatePrice(_oresForBase);
    }

    //public bool BuyMiner(Store store) => Buy(store, _minerPrice);
    //public bool BuyColonist(Store store) => Buy(store, _basePrice);

    //public bool Buy(Store store, Dictionary<string, int> price)
    //{
    //    bool result = store.CheckOresForEnough(price);

    //    if (result)
    //        store.RemoveOres(price);

    //    return result;
    //}

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
