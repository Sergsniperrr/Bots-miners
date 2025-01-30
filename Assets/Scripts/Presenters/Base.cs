using System;
using UnityEngine;

[RequireComponent(typeof(OreDetector))]
[RequireComponent(typeof(Pricelist))]
public class Base : MonoBehaviour, IContainer, IObservable
{
    private MinersHandler _minersHandler;
    private Storage _storage;
    private OreDetector _detector;
    private Buyer _buyer;
    private SelectionAura _aura;
    private bool _isBuilderCreating = false;

    public event Action<Transform> CameraInitialized;

    public Vector3 Position => transform.position;
    public Vector3 FlagPosition { get; private set; }

    private void Awake()
    {
        _detector = GetComponent<OreDetector>();
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
    }

    private void OnEnable()
    {
        _detector.OreDetected += _minersHandler.CollectOre;
    }

    private void OnDisable()
    {
        _detector.OreDetected -= _minersHandler.CollectOre;
    }

    public void ShowAura() => _aura.Show();
    public void HideAura() => _aura.Hide();

    public void SetFlagPosition(Vector3 position)
    {
        FlagPosition = position;
        _isBuilderCreating = true;

        TryCreateBuilder();
    }

    public void ResetFlagPosition()
    {
        FlagPosition = transform.position;
        _isBuilderCreating = false;
    }

    public void AddToStore(Ore ore)
    {
        _storage.Add(ore);

        TryCreateMiner();
        TryCreateBuilder();
    }

    public void InitializeData(Miner minerPrefab, Transform camera,
                               IBaseSpawner baseSpawner, int initialMinersCount = 0)
    {
        _minersHandler.InitializeData(minerPrefab, this, baseSpawner);
        CameraInitialized.Invoke(camera);

        for (int i = 0; i < initialMinersCount; i++)
            _minersHandler.CreateMiner();
    }

    public void AddMiner(Miner miner) => _minersHandler.AddMiner(miner);

    private void TryCreateMiner()
    {
        if (_isBuilderCreating == false && _buyer.TryBuyMiner(_storage))
            _minersHandler.CreateMiner();
    }

    private void TryCreateBuilder()
    {
        if (_isBuilderCreating && _buyer.TryBuyBuilder(_storage))
        {
            _minersHandler.CreateBuilder(FlagPosition);
            _isBuilderCreating = false;
        }
    }
}