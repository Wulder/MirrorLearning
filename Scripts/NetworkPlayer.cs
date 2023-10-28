using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkPlayer : NetworkBehaviour
{

    public override void OnStartClient()
    {
        GameManager.Instance.PlayersManager.AddPlayer(this);

    }

    private void Awake()
    {
        
        
    }

    public override void OnStopClient()
    {
        GameManager.Instance.PlayersManager.RemovePpayer(this);
    }


}
