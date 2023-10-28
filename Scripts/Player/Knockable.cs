using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Knockable : NetworkBehaviour
{
    public event Action OnStartGettingUp;
    public event Action OnStopGettingUp;
    public event Action OnGetUp;
    public float CurrentRevivingProgress => _currentRevivingTime;
    public float ReviveTime => _reviveTime;

    public bool IsReviving => _isReviving;

    [SerializeField] private Health _health;
    [SerializeField] private NetworkAnimatorController _animController;
    [SerializeField] private float _reviveTime, _revivedHealthModify;
    [Header("Disable components")]
    [SerializeField] private NetworkCharacterController _cc;
    [SerializeField] private WeaponManager _wm;
    [SerializeField] private WeaponManagerInputProvider _wmInput;

    [SyncVar] private float _currentRevivingTime;
    [SyncVar] private bool _isReviving;

    private void OnEnable()
    {
        _health.OnDead += Knockout;
    }

    private void OnDisable()
    {
        _health.OnDead -= Knockout;
    }

    
    void Knockout()
    {
        Debug.Log($"Player {name} was knocked!");
        _cc._canMove = false;
        _wm.enabled = false;
        _wmInput.enabled = false;
        _animController.Knockout();
    }

    #region RPCs
    [ClientRpc(includeOwner = true)]
    void RpcGetUp()
    {
        _cc._canMove = true;
        _wm.enabled = true;
        _wmInput.enabled = true;
        if(isLocalPlayer)
        {
            _animController.GetUp();
            _health.CmdSetHealth((int)((float)_health.StartHealth * _revivedHealthModify));
        }
        
        OnGetUp?.Invoke();

    }

    [ClientRpc]
    void RpcStartGettingUp()
    {
            OnStartGettingUp?.Invoke();
    }

    [ClientRpc]
    void RpcStopGettingUp()
    {
        OnStopGettingUp?.Invoke();
    }
    #endregion




    [Command(requiresAuthority = false)]
    public void CmdStartGettingUp()
    {
        if(!_health.IsAlive && !_isReviving)
        {
            _currentRevivingTime = _reviveTime;
            _isReviving = true;
            RpcStartGettingUp();
        }
        
    }

    [Command(requiresAuthority = false)]
    public void CmdStopGettingUp()
    {
        if(!_health.IsAlive && _isReviving)
        {
            RpcStopGettingUp();
            _isReviving = false;

        }

    }

    [Server]
    void Reviving()
    {
        if(_currentRevivingTime > 0)
        {
            _currentRevivingTime -= Time.deltaTime;
        }
        else
        {
            RpcGetUp();
            _isReviving = false;
        }
    }

    private void Update()
    {
        if(_isReviving)
        {
            Reviving();
        }
    }
}
