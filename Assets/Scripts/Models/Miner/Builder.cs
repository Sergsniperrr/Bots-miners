using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Router))]
public class Builder : MonoBehaviour
{
    private Router _router;
    private Vector3 _flagPosition;
    private DemoBase _demoBase;
    private IBaseSpawner _baseSpawner;

    public event Action<Base> BaseWasBuilt;

    private void Awake()
    {
        _router = GetComponent<Router>();
        _demoBase = GetComponentInChildren<DemoBase>();

        if (_demoBase == null)
            throw new NullReferenceException(nameof(_demoBase));

        _demoBase.gameObject.SetActive(false);
    }

    public void BuildNewBase(IBaseSpawner baseSpawner, Vector3 flagPosition)
    {
        float offset = 0.15f;
        _flagPosition = flagPosition;
        _flagPosition.y = offset;
        _demoBase.gameObject.SetActive(true);

        _baseSpawner = baseSpawner ?? throw new ArgumentNullException(nameof(baseSpawner));
        _router.GoToFlag(_flagPosition);

        _router.ArrivedToFlag += CreateBase;
    }

    private void CreateBase()
    {
        _router.ArrivedToFlag -= CreateBase;

        StartCoroutine(DropDemoBase());
    }

    private IEnumerator DropDemoBase()
    {
        float speed = 10f;
        float offset = 5f;
        float maxScaleOnXZ = 2f;
        float maxScaleY = 0.3f;
        Vector3 maxScale = new(maxScaleOnXZ, maxScaleY, maxScaleOnXZ);
        Vector3 currentPosition = _demoBase.transform.position;
        Vector3 currentScale = _demoBase.transform.localScale;

        while (currentPosition != _flagPosition)
        {
            currentPosition = Vector3.MoveTowards(currentPosition, _flagPosition, speed * Time.deltaTime);
            currentScale = Vector3.MoveTowards(currentScale, maxScale, speed * Time.deltaTime);

            currentPosition.y += offset * Time.deltaTime;
            offset = Mathf.MoveTowards(offset, 0f, speed * Time.deltaTime);

            _demoBase.transform.position = currentPosition;
            _demoBase.transform.localScale = currentScale;

            yield return null;
        }

        SpawnBase();
        
        _demoBase.ResetTransform();
        _demoBase.gameObject.SetActive(false);
    }

    private void SpawnBase()
    {
        int initialMinersCount = 0;

        Base newBase = _baseSpawner.CreateBase(_flagPosition, initialMinersCount);

        BaseWasBuilt?.Invoke(newBase);
    }
}
