using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIRevivingProgress : MonoBehaviour
{
    [SerializeField] Reviver _reviver;
    [SerializeField] Image _bar;
    [SerializeField] TextMeshProUGUI _text;

    [Range(0,1)]
    [SerializeField] private float _amount;

    private void OnEnable()
    {
        _reviver.OnStartRevive += Enable;
        _reviver.OnEndRevive += Disable;
        
    }

    private void OnDisable()
    {
        _reviver.OnStartRevive -= Enable;
        _reviver.OnEndRevive -= Disable;
    }

    private void Start()
    {
        Disable();
    }
    void Enable()
    {
        _bar.enabled = true;
        _text.enabled = true;
    }

    void Disable()
    {
        _bar.enabled = false;
        _text.enabled = false;
    }

    private void Update()
    {
        if (_reviver.Knockable)
        {
            _amount = _reviver.Knockable.CurrentRevivingProgress / _reviver.Knockable.ReviveTime;
            _bar.fillAmount = _amount;
            _text.text = _reviver.Knockable.CurrentRevivingProgress.ToString("0.0");
        }
        
    }
}
