using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class DemoBase : MonoBehaviour
{
    private Vector3 _zeroPosition = new(0f, 1f, 0f);
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void Show() => _renderer.enabled = true;

    public void Hide()
    {
        _renderer.enabled = false;
        BackBoZeroPosition();
    }

    private void BackBoZeroPosition() => transform.position = _zeroPosition;
}
