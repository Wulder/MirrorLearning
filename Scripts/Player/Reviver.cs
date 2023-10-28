using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reviver : NetworkBehaviour
{
    public event Action OnStartRevive, OnEndRevive;
    public Knockable Knockable => _knockable;
    private Knockable _knockable;
    private bool _isRevive;

    private void Start()
    {
        _knockable = null;
    }

    private void Update()
    {
        if(isLocalPlayer)
        {
            if(_knockable)
            {
                if(Input.GetKeyDown(KeyCode.F) && !_knockable.IsReviving)
                {
                        _knockable.CmdStartGettingUp();
                        OnStartRevive?.Invoke();
                        _knockable.OnGetUp += EndRevive;
                        _isRevive = true;
                             
                }

                if (Input.GetKeyUp(KeyCode.F) && _isRevive)
                {
                    _knockable.CmdStopGettingUp();
                    OnEndRevive?.Invoke();
                    _isRevive= false;
                }
            }
        }
    }

    void EndRevive()
    {
        OnEndRevive?.Invoke();
        _knockable.OnGetUp -= EndRevive;
    }
    private void OnTriggerStay(Collider other)
    {
        if(_knockable == null)
        {
            if (other.GetComponent<KnockOutTrigger>())
            {
                _knockable = other.GetComponentInParent<Knockable>();
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(_knockable)
        {
            if (other.GetComponent<KnockOutTrigger>())
            {
                _knockable.CmdStopGettingUp();
                EndRevive();
                _knockable = null;
            }
        }
    }

}
