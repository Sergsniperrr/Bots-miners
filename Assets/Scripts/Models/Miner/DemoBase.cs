using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class DemoBase : MonoBehaviour
{
    private Vector3 _zeroPosition = new(0f, 1f, 0f);
    private Vector3 _zeroScale = new(0.8f, 0.12f, 0.8f);

    public void ResetTransform()
    {
        transform.localPosition = _zeroPosition;
        transform.localScale = _zeroScale;
    }
}
