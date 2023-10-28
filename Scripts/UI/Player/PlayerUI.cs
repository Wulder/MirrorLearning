using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerUI : NetworkBehaviour
{
    private NetworkIdentity _netIdentity;

    private void Awake()
    {
        _netIdentity = GetComponentInParent<NetworkIdentity>();
    }
    void Start()
    {
        if(!_netIdentity.isLocalPlayer)
        {
            Destroy(gameObject);
        }
    }
}
