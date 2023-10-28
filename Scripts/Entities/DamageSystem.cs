using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using Unity.VisualScripting;

[RequireComponent(typeof(Health))]
public class DamageSystem : NetworkBehaviour
{

    public Health Health { get { return _health; } private set { } }
    [SerializeField] Health _health;

    [Command(requiresAuthority = false)]
    void CmdDamage(int damage)
    {
        _health.SetDamage(damage);
    }

    public void Damage(int damage)
    {
        CmdDamage(damage);
        
    }

    [ContextMenu("Add receivers")]
    void AddDamageReceivers()
    {
        List<Collider> _colls = GetComponentsInChildren<Collider>().ToList();
        foreach (Collider col in _colls)
        {
            DamageReceiver dr = col.AddComponent<DamageReceiver>();
            dr.Init(1, this);
        }
    }
}
