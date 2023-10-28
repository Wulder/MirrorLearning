using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DamageZone : NetworkBehaviour
{

    [SerializeField] private int _damage;
    [SerializeField] private float _interval;

    private List<Health> _healths = new List<Health>();
    public override void OnStartServer()
    {
        InvokeRepeating(nameof(GiveDamage), _interval, _interval);
    }

    [Server]
    void GiveDamage()
    {
        foreach(Health h in _healths)
        {
            h.SetDamage(_damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Health>())
            _healths.Add(other.GetComponentInParent<Health>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Health>())
            _healths.Remove(other.GetComponentInParent<Health>());
            
    }
}
