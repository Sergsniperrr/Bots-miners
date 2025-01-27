using System;
using UnityEngine;

[RequireComponent(typeof(OreDetector))]
[RequireComponent(typeof(Pricelist))]
public class Base : MonoBehaviour, IContainer, IObservable
{
    [SerializeField] private int _initialMinersCount = 3;

    private MinersHandler _minersHandler;
    private Store _store;
    private OreDetector _detector;
    private Pricelist _pricelist;
    private SelectionAura _aura;

    public event Action<Transform> CameraInitialized;

    public Vector3 Position => transform.position;

    private void Awake()
    {
        _detector = GetComponent<OreDetector>();
        _pricelist = GetComponent<Pricelist>();

        _minersHandler = transform.GetComponentInChildren<MinersHandler>();
        _store = transform.GetComponentInChildren<Store>();
        _aura = transform.GetComponentInChildren<SelectionAura>();

        if (_minersHandler == null)
            throw new NullReferenceException(nameof(_minersHandler));

        if (_store == null)
            throw new NullReferenceException(nameof(_store));

        if (_aura == null)
            throw new NullReferenceException(nameof(_aura));
    }

    private void Start()
    {
        for (int i = 0; i < _initialMinersCount; i++)
            _minersHandler.CreateMiner();
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

    public void AddToStore(Ore ore)
    {
        _store.Add(ore);

        if (_store.TryPay(_pricelist.MinerPrice))
            _minersHandler.CreateMiner();
    }

    public void InitializeData(Miner minerPrefab, Transform camera)
    {
        if (minerPrefab == null)
            throw new ArgumentNullException(nameof(minerPrefab));

        if (camera == null)
            throw new ArgumentNullException(nameof(camera));

        _minersHandler.InitializeData(minerPrefab, this);
        CameraInitialized.Invoke(camera);
    }
}
