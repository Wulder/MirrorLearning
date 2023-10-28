using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    public DamageSystem DamageSystem { get { return _damageSystem; } private set { } }
    [SerializeField] DamageSystem _damageSystem;
    [SerializeField] float _damageModify = 1;

    public void Init(float modify, DamageSystem ds)
    {
        _damageSystem = ds;
        _damageModify = modify;
    }

    public void Damage(int damage)
    {
        
        _damageSystem.Damage((int)((float)damage * _damageModify));
    }
}
