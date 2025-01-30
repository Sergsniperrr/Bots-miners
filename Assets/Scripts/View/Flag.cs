using UnityEngine;

[RequireComponent(typeof(FlagRotator))]
public class Flag : MonoBehaviour
{
    private Renderer[] _renderers;
    private FlagRotator _rotator;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        _rotator = GetComponent<FlagRotator>();
    }

    public void Show() => ChangeVisibility(true);
    public void Hide() => ChangeVisibility(false);
    public void InitializeCamera(Transform camera) => _rotator.InitializeCamera(camera);

    private void ChangeVisibility(bool isVisible)
    {
        foreach (Renderer renderer in _renderers)
            renderer.enabled = isVisible;
    }
}
