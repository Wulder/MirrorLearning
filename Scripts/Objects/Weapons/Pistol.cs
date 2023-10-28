using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Pistol : Weapon, IAiming, IShootable
{

    public event UnityAction<Vector3, RaycastHit> OnHit;
    public event UnityAction<Vector3, Vector3> OnShoot;

    Transform IShootable.ShootSource { get { return _shootSource; } set { } }

    [SerializeField] private Transform _leftHandGrab, _leftElbowHint, _shootSource;
    [SerializeField] private float _aimInterpolation, _constraitInterpolation;
    [SerializeField] private float _fireRate, _maxShootDistance, _bulletSpeed, _bulletMass;
    [SerializeField] private int _damage;
    [SerializeField] private bool _isAutomatic;
    [SerializeField] private ShootingSound _shootingSound;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _ui;
    [SerializeField] private PhysicsCrosshair _crosshair;

    [SerializeField] LayerMask _hitMask;

    private PlayerCamera _camera;
    private bool _beginShooting, _shooting, _shootingEnd;
    private float _shootingCoolDown;

    private Vector3 _shootTarget = Vector3.zero;
    private RaycastHit _hit;

    public float FireRate { get => _fireRate; set { } }

    public bool IsAutomatic { get => _isAutomatic; set { } }

  
   

    private void Start()
    {
        _camera = GetComponentInParent<PlayerCamera>();
        _shootingCoolDown = 0;
        _animator.SetFloat("FireRate", _fireRate);
        if(!_weaponManager.isLocalPlayer)
        {
            Destroy(_ui);
        }
    }


    public void BeginAim()
    {
        _weaponManager.Rigging.Interpolation = _constraitInterpolation;
        _weaponManager.Rigging.SetLeftHandGrab(_leftHandGrab);
        _weaponManager.Rigging.SetLeftElbowHint(_leftElbowHint);
        _weaponManager.netAnimator.Aiming(true);
        _weaponManager.netAnimator.Sprint(false);
        _weaponManager.Rigging.StartSmoothWeightUp(2);
        


        if (_weaponManager.isLocalPlayer)
        {
            _camera.AimCamController.ResetDirection();
            _camera.SetHighestPriority(_camera.AimCamera);
            _crosshair.Show();
           
        }
    }
    public void Aim(Vector3 point)
    {
        _weaponManager.Rigging.AimTarget.position = Vector3.Lerp(_weaponManager.Rigging.AimTarget.position, point, _aimInterpolation);
        _shootTarget = point;

        if(_weaponManager.isLocalPlayer)
        {
            Physics.Raycast(_shootSource.position, point - _shootSource.position, out RaycastHit _realAimPoint, 10000, _weaponManager.AimLayerMask);
            _crosshair.SetPosition(_realAimPoint.point);
        }
        
    }
    public void AimEnd()
    {
        _weaponManager.netAnimator.Aiming(false);
        _weaponManager.Rigging.ResetTargets();
        _weaponManager.Rigging.ResetInterpolation();
        _weaponManager.Rigging.StartSmoothWeightDown(2);
        if (_weaponManager.isLocalPlayer)
        {
            _camera.SetHighestPriority(_camera.FreeLookCamera);
            _camera.FreeLookCamera.ForceCameraPosition(Camera.main.transform.position, Camera.main.transform.rotation);
            _crosshair.Hide();
        }
    }

    public void BeginShooting()
    {
        _beginShooting = true;
        _shootingEnd = false;
        _shooting = true;

    }
    public void ShootingEnd()
    {
        if (IsAutomatic)
            _shootingSound.Play(true);

        _shooting = false;
        _beginShooting = false;
        _shootingEnd = true;
    }
    public void Shoot()
    {
        OnShoot?.Invoke(_shootSource.position,_shootTarget);
        _animator.SetTrigger("Shoot");
        _animator.SetTrigger("Shoot");

        if (Physics.Raycast(_shootSource.position, _shootTarget - _shootSource.position, out _hit, _maxShootDistance, _hitMask))
        {
            Hit();
        }

        _shooting = true;
    }

    void Hit()
    {
        OnHit?.Invoke(_shootSource.position, _hit);

        if (_weaponManager.isLocalPlayer)
        {
            DamageReceiver dr;
            if (_hit.collider.TryGetComponent<DamageReceiver>(out dr))
            {
                dr.Damage(_damage);

                if((dr.DamageSystem.Health.CurrentHealth - _damage) <= 0)
                    dr.DamageSystem.Health.OnDead += () => _hit.collider.GetComponent<Rigidbody>().AddForceAtPosition((_hit.point - _shootSource.position).normalized * _bulletSpeed * _bulletMass, _hit.point, ForceMode.Impulse);
            }

        }

        if (_hit.collider.GetComponent<Rigidbody>())
        {
            _hit.collider.GetComponent<Rigidbody>().AddForceAtPosition((_hit.point - _shootSource.position).normalized * _bulletSpeed * _bulletMass, _hit.point, ForceMode.Impulse);
        }


    }

    void Update()
    {
        if (_shootingCoolDown > 0)
        {
            _shootingCoolDown -= Time.deltaTime * FireRate;
        } //TIMER

        if (_shootingCoolDown <= 0)
        {
            if (_shooting)
            {
                Shoot();

                _shootingCoolDown = 1;

                if (!IsAutomatic)
                    _shooting = false;

                _shootingSound.Play(!IsAutomatic);
            }


        }
    }


}
