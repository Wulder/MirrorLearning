using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;

public abstract class NPC : NetworkBehaviour
{

    public NPCType NpcType;
    public Animator Animator;

    [Server]
    public virtual void Spawn()
    {

    }
    [Server]
    public virtual void Despawn()
    {
        
    }

    public bool GetNearPlayer(float distance, out GameObject player)
    {
        
        foreach (GameObject p in GameManager.Instance.PlayersManager.PlayerObjects)
        {
            if(Vector3.Distance(transform.position, p.transform.position) <= distance)
            {
                player = p;
                return true;
            }
        }
        player = null;
        return false;
    }

    public bool GetNearPlayerInRange(float minDist, float maxDist, out GameObject player)
    {
        
        foreach (GameObject p in GameManager.Instance.PlayersManager.PlayerObjects)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);
            if ((dist > minDist) && (dist < maxDist))
            {
                player = p;
                return true;
            }
        }
        player = null;
        return false;
    }


}
