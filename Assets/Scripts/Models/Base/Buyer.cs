using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pricelist))]
public class Buyer : MonoBehaviour
{
    private Pricelist _pricelist;

    private void Awake()
    {
        _pricelist = GetComponent<Pricelist>();
    }

    public bool TryBuyMiner(IOreRemover _storage) => TryBuy(_storage, _pricelist.MinerPrice);
    public bool TryBuyBuilder(IOreRemover _storage) => TryBuy(_storage, _pricelist.BuilderPrice);

    private bool TryBuy(IOreRemover _storage, Dictionary<string, int> price)
    {
        return _storage.TryRemoveOres(price);
    }
}
