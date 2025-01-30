using UnityEngine;

public interface IBaseSpawner
{
    Base CreateBase(Vector3 position, int initialMinersCount);
}
