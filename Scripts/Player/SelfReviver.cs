using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfReviver : NetworkBehaviour
{
    [SerializeField] private Knockable _knockable;
    private bool _startReviving;
    private void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.R) && !_knockable.IsReviving)
            {
                _knockable.CmdStartGettingUp();
                _startReviving = true;
                
            }
            if(Input.GetKeyUp(KeyCode.R) && _startReviving)
            {
                _knockable.CmdStopGettingUp();
                _startReviving=false;
            }
            
        }
    }
}
