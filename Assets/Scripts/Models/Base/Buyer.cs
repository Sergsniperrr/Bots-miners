using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pricelist))]
public class Buyer : MonoBehaviour
{
    private Pricelist _pricelist;
    private IOreRemover _storage;

    private void Awake()
    {
        _pricelist = GetComponent<Pricelist>();
    }

    public void InitializeData(IOreRemover store) =>
        _storage = store ?? throw new ArgumentNullException(nameof(store));

    public bool TryBuyMiner() =>
        TryBuy(_pricelist.MinerPrice);

    public bool TryBuyBase() =>
        TryBuy(_pricelist.BasePrice);

    public bool CheckEnoughOresForBase() =>
        _storage.CheckOresForEnough(_pricelist.BasePrice);

    private bool TryBuy(Dictionary<string, int> price) =>
        _storage.TryRemoveOres(price);
}