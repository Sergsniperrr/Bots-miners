using UnityEngine;

public class SelectedBaseView : MonoBehaviour
{
    [SerializeField] private Flag _flag;
    [SerializeField] private Transform _camera;

    private void Start()
    {
        _flag.gameObject.SetActive(false);
        _flag.InitializeCamera(_camera);
    }

    public void ShowSelectedBase(Base selectedBase)
    {
        selectedBase.ShowAura();

        _flag.gameObject.SetActive(true);
        _flag.transform.position = selectedBase.FlagPosition;
    }

    public void UnselectBase(Base targetBase)
    {
        targetBase.HideAura();
        _flag.gameObject.SetActive(false);
    }
}
