using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MinerSearcher
{
    public static Miner FindNearestFreeMiner(Miner[] miners, Vector3 targetPoint)
    {
        var freeMiners = miners.Where(miner => miner.IsFree);

        return GetNearestMiner(targetPoint, freeMiners);
    }

    private static Miner GetNearestMiner(Vector3 targetPoint, IEnumerable<Miner> freeMiners)
    {
        if (freeMiners.Count() == 0)
            return null;

        Miner nearestMiner = null;
        float minSqrDistance = float.MaxValue;
        float sqrDistance;

        foreach (Miner miner in freeMiners)
        {
            sqrDistance = CalculateSquareOfDistance(miner, targetPoint);

            if (minSqrDistance > sqrDistance)
            {
                minSqrDistance = sqrDistance;
                nearestMiner = miner;
            }
        }

        return nearestMiner;
    }

    private static float CalculateSquareOfDistance(Miner miner, Vector3 targetPoint)
    {
        if (miner == null)
            throw new ArgumentNullException(nameof(miner));

        return Vector3.SqrMagnitude(miner.transform.position - targetPoint);
    }
}
