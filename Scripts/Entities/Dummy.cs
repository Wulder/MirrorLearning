using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Dummy : NPC
{

    public NavMeshAgent NMAgent => _nmAgent;
    public GameObject FocusedPlayer => _focusedPlayer;

    [SerializeField] private Health _health;
    [SerializeField] private RagdollController _ragdollController;
    [SerializeField] private NavMeshAgent _nmAgent;
    [SerializeField] private float _despawnDelaySec =5;

    private GameObject _focusedPlayer;

    
    private void OnEnable()
    {
        _health.OnChange += GetDamage;
    }

    private void OnDisable() 
    {
        _health.OnChange -= GetDamage;
    }

    void GetDamage(int oldVal, int newVal)
    {
        
    }

    public void Kill()
    {
        _ragdollController.Enable();
        Despawn();
    
    }

    public void SetFocusPlayer(GameObject player)
    {
        _focusedPlayer = player;
    }

    [Server]
    public override void Spawn()
    {
        Instantiate(gameObject, Vector3.zero, Quaternion.identity);
    }

    [Server]
    public override void Despawn()
    {
        StartCoroutine(CDespawn());
    }

    [Server]
    IEnumerator CDespawn()
    {
        yield return new WaitForSeconds(_despawnDelaySec);
        NetworkServer.Destroy(gameObject);
        yield return null;
    }
}
