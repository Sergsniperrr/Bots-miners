using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OreStacker))]
public class Storage : MonoBehaviour, IOreRemover
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

    public bool TryRemoveOres(Dictionary<string, int> removableOres)
    {
        if (_stacker.CheckOresForEnough(removableOres) == false)
            return false;

        foreach (KeyValuePair<string, int> ore in removableOres)
            _stacker.Remove(ore);

        return true;
    }
}
