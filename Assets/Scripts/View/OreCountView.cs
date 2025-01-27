using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OreCountView : MonoBehaviour
{
    private readonly string _title = "  �����\n ����\n";

    private IOreCounter _counter;
    private Text _text;

    private void Awake()
    {
        _counter = transform.parent.GetComponentInChildren<IOreCounter>();
        _text = GetComponentInChildren<Text>();

        if (_counter == null)
            throw new MissingComponentException(nameof(IOreCounter));

        if (_text == null)
            throw new MissingComponentException(nameof(Text));

        _text.text = $"{_title}   �����";
    }

    private void OnEnable()
    {
        _counter.OresCountChanged += ChangeText;
    }

    private void OnDisable()
    {
        _counter.OresCountChanged -= ChangeText;
    }

    private void ChangeText(Dictionary<string, int> counter)
    {
        string text = _title;

        foreach (KeyValuePair<string, int> ore in counter)
            text += $"{ore.Key} : {ore.Value}\n";

        _text.text = text;
    }
}
