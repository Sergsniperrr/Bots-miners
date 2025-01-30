using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MinerSpawner))]
public class MinersHandler : MonoBehaviour
{
    [SerializeField] private float _radiusOfWaitingArea = 4f;

    private List<Miner> _miners = new();
    private MinerSpawner _minerSpawner;
    private IBaseSpawner _baseSpawner;
    private Miner _freeMiner;
    private IContainer _mainBase;

    private void Awake()
    {
        _minerSpawner = GetComponent<MinerSpawner>();
    }

    public void InitializeData(Miner prefab, IContainer mainBase, IBaseSpawner baseSpawner)
    {
        _mainBase = mainBase ?? throw new ArgumentNullException(nameof(mainBase));
        _baseSpawner = baseSpawner ?? throw new ArgumentNullException(nameof(baseSpawner));

        _minerSpawner.InitializeData(prefab);
    }

    public void CreateMiner()
    {
        AddMiner(_minerSpawner.CreateMiner());
        UpdateWaitingPoints();
    }

    public void CreateBuilder(Vector3 flagPosition)
    {
        Miner builder = _minerSpawner.CreateMiner();
        builder.BuildNewBase(_baseSpawner, flagPosition);
    }

    public void CollectOre(Ore ore)
    {
        _freeMiner = MinerSearcher.FindNearestFreeMiner(_miners.ToArray(), ore);

        if (_freeMiner != null)
        {
            if (ore.TryGetComponent(out Collider collider))
                Destroy(collider);

            _freeMiner.Collect(ore);
        }
    }

    public void AddMiner(Miner miner)
    {
        miner.transform.SetParent(transform);
        miner.SetMainBase(_mainBase);

        _miners.Add(miner);
    }

    private void UpdateWaitingPoints()
    {
        if (_miners.Count == 0)
            return;

        Vector3[] waitingPoints = WaitingPointCreator.Create(transform.position, _miners.Count, _radiusOfWaitingArea);

        for (int i = 0; i < _miners.Count; i++)
            _miners[i].SetWaitingPoint(waitingPoints[i]);
    }
}