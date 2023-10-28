using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockOutTrigger : MonoBehaviour
{
    public Knockable Knockable => _knockable;
    [SerializeField] NetworkIdentity _networkIdentity;
    [SerializeField] private Knockable _knockable;

    private void Start()
    {
        if (_networkIdentity.isLocalPlayer)
            Destroy(gameObject);
        else
            GetComponent<Collider>().enabled = true;
    }

}
