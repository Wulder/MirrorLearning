using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistDamagerController : NetworkBehaviour
{
    [SerializeField] private FistDamager _fistDamager;
    void Start()
    {
        _fistDamager.Disable();
    }

    
    public void Kick()
    {
        if(isServer)
        {
            _fistDamager.Enable();
        }
        
    }

    public void KickEnd()
    {
        if (isServer)
        {
            _fistDamager.Disable();
        }

    }
}
