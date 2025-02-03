using System;
using System.Collections.Generic;
using System.Linq;
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
    private IBaseBuilder _baseBuilder;
    private bool _isNeedSendBuilder = false;

    public event Action FreeMinersOver;
    public event Action BuilderBeenSent;

    public int MinersCount => _miners.Count;

    private void Awake()
    {
        _minerSpawner = GetComponent<MinerSpawner>();
    }

    public void InitializeData(Miner prefab, IContainer mainBase, IBaseSpawner baseSpawner, IBaseBuilder baseBuilder)
    {
        _mainBase = mainBase ?? throw new ArgumentNullException(nameof(mainBase));
        _baseSpawner = baseSpawner ?? throw new ArgumentNullException(nameof(baseSpawner));
        _baseBuilder = baseBuilder ?? throw new ArgumentNullException(nameof(baseBuilder));

        _minerSpawner.InitializeData(prefab);
    }

    public void SpawnMiner() =>
        AddMiner(_minerSpawner.CreateMiner());

    public void StartSendingBuilder() =>
        _isNeedSendBuilder = true;

    public void CancelSendingBuilder() =>
        _isNeedSendBuilder = false;

    public void GiveTaskToMiner(Ore ore)
    {
        if (_isNeedSendBuilder)
            SendBuilder();
        else
            CollectOre(ore);
    }

    public void AddMiner(Miner miner)
    {
        miner.transform.SetParent(transform);
        miner.SetMainBase(_mainBase);

        _miners.Add(miner);
        UpdateWaitingPoints();
    }

    public bool CheckForNoFreeMiners()
    {
        var freeMiners = _miners.Where(miner => miner.IsFree);

        return freeMiners.Count() == 0;
    }

    public void SendBuilder()
    {
        _freeMiner = GetNearestFreeMiner(_baseBuilder.FlagPosition);

        if (_freeMiner != null)
        {
            _miners.Remove(_freeMiner);
            _freeMiner.BuildNewBase(_baseSpawner, _baseBuilder.FlagPosition);
            _isNeedSendBuilder = false;

            BuilderBeenSent?.Invoke();
        }
    }

    private void UpdateWaitingPoints()
    {
        if (_miners.Count == 0)
            return;

        Vector3[] waitingPoints;
        waitingPoints = WaitingPointCreator.Create(transform.position, _miners.Count, _radiusOfWaitingArea);

        for (int i = 0; i < _miners.Count; i++)
            _miners[i].SetWaitingPoint(waitingPoints[i]);
    }

    private void CollectOre(Ore ore)
    {
        _freeMiner = GetNearestFreeMiner(ore.transform.position);

        if (_freeMiner != null)
        {
            if (ore.TryGetComponent(out Collider collider))
                Destroy(collider);

            _freeMiner.Collect(ore);

            if (CheckForNoFreeMiners())
                FreeMinersOver?.Invoke();
        }
    }

    private Miner GetNearestFreeMiner(Vector3 position) =>
        MinerSearcher.FindNearestFreeMiner(_miners.ToArray(), position);
}