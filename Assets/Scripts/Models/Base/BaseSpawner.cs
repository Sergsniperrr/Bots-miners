using UnityEngine;

public class BaseSpawner : MonoBehaviour, IBaseSpawner
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private Miner _minerPrefab;
    [SerializeField] private Transform _camera;
    [SerializeField] private int _initialMinersCount = 3;

    private void Start()
    {
        CreateBase(Vector3.zero, _initialMinersCount);
    }

    public Base CreateBase(Vector3 position, int initialMinersCount = 0)
    {
        float offsetY = 0.15f;
        position.y = offsetY;

        Base baseClone = Instantiate(_basePrefab, position, transform.rotation);

        baseClone.transform.SetParent(transform);
        baseClone.InitializeData(_minerPrefab, _camera, this, initialMinersCount);

        return baseClone;
    }
}
