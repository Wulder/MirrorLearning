using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Spawner : NetworkBehaviour
{
    [SerializeField] NPC _prefabNpc;

    private void Awake()
    {
        
    }

    [Server, ContextMenu("Spawn")]
    void Spawn()
    {
        NPC npc = Instantiate(_prefabNpc, new Vector3(Random.RandomRange(-3,3), Random.RandomRange(0, 3), Random.RandomRange(-3, 3)), Quaternion.identity);
        NetworkServer.Spawn(npc.gameObject);
    }

    [Server]
    public override void OnStartServer()
    {
        Debug.Log("start server");
        Spawn();
    }

    

   

}
