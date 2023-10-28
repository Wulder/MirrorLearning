using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private protected Weapons id;
    public Weapons ID { get { return id; } private set { } }

    private protected WeaponManager _weaponManager;

    public void Init(WeaponManager w)
    {
        _weaponManager = w;
    }
}







