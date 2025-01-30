using UnityEngine;

public class SelectedBaseView : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private Transform _camera;

    private Flag _flag;

    private void Start()
    {
        _flag = Instantiate(_flagPrefab, transform.position, transform.rotation);
        _flag.Hide();
        _flag.InitializeCamera(_camera);
    }

    public void ShowSelectedBase(Base selectedBase)
    {
        selectedBase.ShowAura();
        _flag.Show();
        _flag.transform.position = selectedBase.FlagPosition;
    }

    public void UnselectBase(Base targetBase)
    {
        targetBase.HideAura();
        _flag.Hide();
    }
}
