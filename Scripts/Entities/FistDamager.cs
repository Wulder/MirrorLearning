using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

public class FistDamager : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private Collider _collider;

    [SerializeField] LayerMask _mask;

    private bool _isActive;

    public void Enable()
    {
        _collider.enabled = true;
        _isActive = true;
    }

    public void Disable()
    {
        _collider.enabled = false;
        _isActive = false;
    }

    
    void OnTriggerEnter(Collider other)
    {
        if (!_isActive || (_mask & (1 << other.gameObject.layer)) == 0) return;
        Health _health = other.GetComponentInParent<Health>();
        if(_health)
        {
            if(_health.IsAlive)
            {
                _health.SetDamage(_damage);
                Disable();
            }
            
        }
    }

    
}
