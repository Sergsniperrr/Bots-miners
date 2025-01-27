using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OreStacker))]
public class Store : MonoBehaviour
{
    private OreStacker _stacker;

    private void Awake()
    {
        _stacker = GetComponent<OreStacker>();
    }

    public void Add(Ore ore)
    {
        ore.transform.SetParent(transform);

        _stacker.Add(ore);
    }

    public bool TryPay(Dictionary<string, int> requireOres)
    {
        if (_stacker.CheckOresForEnough(requireOres) == false)
            return false;

        foreach (KeyValuePair<string, int> ore in requireOres)
            _stacker.Remove(ore);

        return true;
    }


}
