using Mirror;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class WeaponManager : NetworkBehaviour
{
    public event Action OnBeginShoot, OnShoot, OnShootEnd;
    public AimRigging Rigging { get { return _aimRigging; } private set { } }
    public NetworkAnimatorController netAnimator { get { return _netAnimatorController; } private set { } }
    public bool HasAiming { get { return _iAiming == null ? false : true; } private set { } }
    public bool HasShootable { get { return _Shootable == null ? false : true; } private set { } }
    public RaycastHit AimHit => _aimHit;
    public LayerMask AimLayerMask => _aimLayersMask;


    [SerializeField] List<Weapon> _weaponsPrefabs = new List<Weapon>();

    [Header("Animation")]
    [SerializeField] NetworkAnimatorController _netAnimatorController;
    [SerializeField] AimRigging _aimRigging;

    [Header("Attach points")]
    [SerializeField] Transform _rightHand;
    [SerializeField] Transform _leftHand;

    [Header("Input")]
    [SerializeField] WeaponManagerInputProvider _inputProvider;

    [Header("Configuration")]
    [SerializeField] float _aimTargetMaxDistance = 100, _unHitAimTargetLength;
    [SerializeField] LayerMask _aimLayersMask;

    [Header("Debug")]
    [SerializeField] bool _drawCameraRaycast;



    #region Input
    [SyncVar(hook = nameof(SetWeapon))] int _weapondID;
    [SyncVar] Vector3 _aimTarget = Vector3.zero;
    [SyncVar] bool _leftMouseButton;
    [SyncVar] bool _rightMouseButton;
    #endregion


    private Weapon _currentWeapon;
    private IAiming _iAiming;

    private IShootable _Shootable;
    private float _shootCoolDownTimer;

    private RaycastHit _aimHit;
    private RaycastHit _realAimHit;


    public override void OnStartClient()
    {
        SetWeapon(0, _weapondID);
        _inputProvider.OnSelectedWeapon += CmdChangeWeaponId;
    }

    #region CmdInput
    [Command]
    void CmdChangeWeaponId(int id)
    {
        _weapondID = id;
    }

    [Command]
    void CmdChangeLeftMouseButton(bool val)
    {
        _leftMouseButton = val;
    }

    [Command]
    void CmdChangeRightMouseButton(bool val)
    {
        _rightMouseButton = val;
    }

    [Command]
    void CmdChangeAimTarget(Vector3 point)
    {
        _aimTarget = point;
    }


    #endregion


    #region CmdAiming
    [Command(requiresAuthority = false)]
    void CmdBeginAim()
    {
        RpcBeginAim();
    }

    [Command(requiresAuthority = false)]
    void CmdAim(Vector3 dir)
    {
        RpcAim(dir);
    }

    [Command(requiresAuthority = false)]
    void CmdAimEnd()
    {
        RpcAimEnd();
    }


    #endregion

    #region RpcAiming
    [ClientRpc(includeOwner = false)]
    void RpcBeginAim()
    {
        BeginAim();
    }

    [ClientRpc(includeOwner = false)]
    void RpcAim(Vector3 dir)
    {
        Aim(dir);
    }

    [ClientRpc(includeOwner = false)]
    void RpcAimEnd()
    {
        AimEnd();
    }


    #endregion

    #region Aiming
    void BeginAim()
    {
        _iAiming?.BeginAim();
    }

    void Aim(Vector3 point)
    {
        _iAiming?.Aim(point);
    }

    void AimEnd()
    {
        _iAiming?.AimEnd();
    }


    #endregion


    #region CmdShooting
    [Command(requiresAuthority = false)]
    void CmdBeginShoot()
    {
        RpcBeginShoot();
    }

    [Command(requiresAuthority = false)]
    void CmdShoot()
    {
        RpcShoot();
    }

    [Command(requiresAuthority = false)]
    void CmdShootEnd()
    {
        RpcShootEnd();
    }


    #endregion

    #region RpcShooting
    [ClientRpc(includeOwner = false)]
    void RpcBeginShoot()
    {
        BeginShoot();
    }

    [ClientRpc(includeOwner = false)]
    void RpcShoot()
    {
        Shoot();
    }

    [ClientRpc(includeOwner = false)]
    void RpcShootEnd()
    {
        ShootEnd();
    }


    #endregion

    #region Shooting
    void BeginShoot()
    {
        OnBeginShoot?.Invoke();
        _Shootable?.BeginShooting();
    }

    void Shoot()
    {
        OnShoot?.Invoke();
        // _Shootable.Shoot();
    }

    void ShootEnd()
    {
        OnShootEnd?.Invoke();
        _Shootable.ShootingEnd();
    }


    #endregion


    #region WeaponChange
    void SetWeapon(int oldvalue, int id)
    {
        Weapon prefab = _weaponsPrefabs.Find(w => w.ID == (Weapons)id);

        if (prefab)
        {
            UnsetWeapon();
            _currentWeapon = Instantiate(prefab, _rightHand);
            _currentWeapon.Init(this);
            _weapondID = (int)_currentWeapon.ID;
            _iAiming = _currentWeapon.GetComponent<IAiming>();
            _Shootable = _currentWeapon.GetComponent<IShootable>();
        }
        else
        {
            Debug.LogWarning($"Weapon with id ({id}) doesn't exists");
        }
    }

    void UnsetWeapon()
    {
        if (_currentWeapon)
        {
            _iAiming?.AimEnd();
            _Shootable?.ShootingEnd();
            Rigging.SetRigWeight(0);
            Rigging.ResetTargets();
            Destroy(_currentWeapon.gameObject);
            _iAiming = null;
            _Shootable = null;
        }
    }

    #endregion



    private void Update()
    {
        if (isLocalPlayer)
        {
            if (_iAiming != null)
            {
                HandleAimInput();
                if (_Shootable != null)
                    HandleShootInput();
            }
                

            
        }

    }

    void HandleAimInput()
    {
        if (_inputProvider.BeginAim)
        {
            CmdChangeRightMouseButton(true);
            CmdBeginAim();
            BeginAim();
            return;
        }

        if (_inputProvider.AimEnd)
        {
            CmdChangeRightMouseButton(false);
            CmdAimEnd();
            AimEnd();
            return;
        }

        if (_inputProvider.Aim)
        {
            Vector3 point;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out _aimHit, _aimTargetMaxDistance, _aimLayersMask))
            {
                point = _aimHit.point;
                CmdChangeAimTarget(point);
            }
            else
            {
                point = Camera.main.transform.position + Camera.main.transform.forward * _unHitAimTargetLength;
                CmdChangeAimTarget(point);
            }
            
            CmdAim(point);

            Aim(point);
            return;
        }
    }

    void HandleShootInput()
    {
        if (_inputProvider.BeginShooting)
        {
            CmdChangeLeftMouseButton(true);
            CmdBeginShoot();
            BeginShoot();
            return;
        }

        if (_inputProvider.ShootingEnd)
        {
            CmdChangeRightMouseButton(false);
            CmdShootEnd();
            ShootEnd();
            return;
        }

       


    }

    private void OnDrawGizmos()
    {
        if (_drawCameraRaycast)
        {
            if (_aimHit.collider)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(Camera.main.transform.position, _aimTarget);
                Gizmos.DrawSphere(_aimTarget, 0.1f);

                Gizmos.color = Color.blue;

                Gizmos.DrawLine(Camera.main.transform.position, _aimHit.point);
                Gizmos.DrawSphere(_aimHit.point, 0.05f);
            }
            else
            {
                Gizmos.color = Color.blue;

                Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * _unHitAimTargetLength);
                Gizmos.DrawSphere(Camera.main.transform.position + Camera.main.transform.forward * _unHitAimTargetLength, 0.1f);
            }



        }
    }
}



public enum Weapons
{
    Hands = 0,
    Pistol = 1,
    Rifle = 2
}
