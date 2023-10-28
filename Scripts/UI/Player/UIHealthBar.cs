using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private RectTransform _bar;
    [SerializeField] private float _maxValue;

    private void OnEnable()
    {
        _health.OnChange += UpdateValue;
    }

    private void OnDisable()
    {
        _health.OnChange -= UpdateValue;
    }
    void UpdateValue(int oldVal, int newVal)
    {
        _bar.sizeDelta = new Vector2(((float)newVal/(float)_health.StartHealth) * _maxValue,_bar.sizeDelta.y);
    }
}
