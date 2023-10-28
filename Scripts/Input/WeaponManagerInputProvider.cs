using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManagerInputProvider : MonoBehaviour
{
    [HideInInspector]
    public bool BeginAim, Aim, AimEnd;
    [HideInInspector]
    public bool BeginShooting, Shooting, ShootingEnd;

    public event UnityAction<int> OnSelectedWeapon;

    [SerializeField] WeaponManager _weaponManager;

    private NetworkPlayer _player;

    private void Start()
    {
        _player = GetComponent<NetworkPlayer>();
    }
    void Update()
    {
        if(_player.isLocalPlayer)
        {
            WeaponSelect();

            if(_weaponManager.HasAiming)
                AimInput();

            if (_weaponManager.HasShootable)
                ShootInput();

        }
    }

    void WeaponSelect()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnSelectedWeapon?.Invoke(1);

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            OnSelectedWeapon?.Invoke(2);
        }
    }
    void AimInput()
    {
        BeginAim = Input.GetMouseButtonDown(1);
        Aim = Input.GetMouseButton(1);
        AimEnd= Input.GetMouseButtonUp(1);

        
    }

    void ShootInput()
    {
        BeginShooting = Input.GetMouseButtonDown(0);
        Shooting = Input.GetMouseButton(0);
        ShootingEnd = Input.GetMouseButtonUp(0);
    }
}
