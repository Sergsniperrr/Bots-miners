using System;
using UnityEngine;

[RequireComponent(typeof(OreDetector))]
[RequireComponent(typeof(Pricelist))]
public class Base : MonoBehaviour, IContainer, IObservable, IBaseBuilder
{
    private readonly int _minMinersCount = 2;

    private MinersHandler _minersHandler;
    private Storage _storage;
    private Buyer _buyer;
    private SelectionAura _aura;
    private OreDetector _OreDetector;

    public event Action<Transform> CameraInitialized;

    public bool IsNewBaseBuilding { get; private set; } = false;
    public Vector3 FlagPosition { get; private set; }
    public Vector3 Position => transform.position;

    private void Awake()
    {
        _OreDetector = GetComponent<OreDetector>();
        _buyer = GetComponent<Buyer>();
        FlagPosition = transform.position;

        _minersHandler = transform.GetComponentInChildren<MinersHandler>();
        _storage = transform.GetComponentInChildren<Storage>();
        _aura = transform.GetComponentInChildren<SelectionAura>();

        if (_minersHandler == null)
            throw new NullReferenceException(nameof(_minersHandler));

        if (_storage == null)
            throw new NullReferenceException(nameof(_storage));

        if (_aura == null)
            throw new NullReferenceException(nameof(_aura));

        HideAura();
        _buyer.InitializeData(_storage);
    }

    private void OnEnable()
    {
        _OreDetector.OreDetected += _minersHandler.GiveTaskToMiner;
        _minersHandler.FreeMinersOver += _OreDetector.DisableScanning;
    }

    private void OnDisable()
    {
        _OreDetector.OreDetected -= _minersHandler.GiveTaskToMiner;
        _minersHandler.FreeMinersOver -= _OreDetector.DisableScanning;
    }

    public void ShowAura() =>
        _aura.gameObject.SetActive(true);

    public void HideAura() =>
        _aura.gameObject.SetActive(false);

    public void SetFlagPosition(Vector3 position)
    {
        int minMinersCount = 2;

        if (_minersHandler.MinersCount < minMinersCount)
            return;

        FlagPosition = position;
        IsNewBaseBuilding = true;

        TrySendBuilder();
    }

    public void ResetFlagPosition()
    {
        _minersHandler.BuilderBeenSent -= BuyBase;

        _minersHandler.CancelSendingBuilder();
        FlagPosition = transform.position;
        IsNewBaseBuilding = false;
    }

    public void AddToStore(Ore ore)
    {
        _storage.Add(ore);
        _OreDetector.EnableScanning();

        TrySendBuilder();
        TrySpawnMiner();
    }

    public void InitializeData(Miner minerPrefab, Transform camera,
                               IBaseSpawner baseSpawner, int initialMinersCount = 0)
    {
        _minersHandler.InitializeData(minerPrefab, this, baseSpawner, this);
        CameraInitialized.Invoke(camera);

        for (int i = 0; i < initialMinersCount; i++)
            _minersHandler.SpawnMiner();
    }

    public void AddMiner(Miner miner)
    {
        _minersHandler.AddMiner(miner);
        _OreDetector.EnableScanning();
    }

    private void BuyBase()
    {
        _minersHandler.BuilderBeenSent -= BuyBase;

        _buyer.TryBuyBase();
        IsNewBaseBuilding = false;
    }

    private void TrySpawnMiner()
    {
        if (IsNewBaseBuilding && _minersHandler.MinersCount >= _minMinersCount)
            return;

        if (_buyer.TryBuyMiner())
            _minersHandler.SpawnMiner();
    }

    private void TrySendBuilder()
    {
        if (IsNewBaseBuilding == false || _minersHandler.MinersCount < _minMinersCount)
            return;

        if (_buyer.CheckEnoughOresForBase() == false)
            return;

        _minersHandler.StartSendingBuilder();

        if (_minersHandler.CheckForNoFreeMiners())
        {
            _minersHandler.BuilderBeenSent += BuyBase;
        }
        else
        {
            BuyBase();
            _minersHandler.SendBuilder();
        }
    }
}