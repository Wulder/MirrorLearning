using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPC))]
public class NPCStateMachine : NetworkBehaviour
{
    public NPC Npc => _npc;
    public NPCState CurrentState => _currentState;

    [SerializeField] private NPCState _initState;
    [SerializeField] private List<NPCState> _states = new List<NPCState>();
    [SerializeField] private NPC _npc;
    private NPCState _currentState;

    [SyncVar] private int _currentStateIndex;


    public override void OnStartServer()
    {
        Init();
    }

    public override void OnStartClient()
    {
        Init(_currentStateIndex);
    }

    void Init()
    {
        _currentState = _initState;
        _currentState.Init(this);
        _currentState.Enter();
    }

    [Client]
    void Init(int index)
    {
        if (!isServer)
        {
            _currentState = _states[index];
            _currentState.Init(this);
            _currentState.ClientEnter();
        }

    }

    [Server]
    public void ChangeState(NPCState newState)
    {
            _currentState.Exit();
            _currentState = newState;
            _currentState.Init(this);
            _currentState.Enter();
            _currentStateIndex = _states.IndexOf(newState);
            RpcChangeState(_currentStateIndex);
    }

    [ClientRpc]
    void RpcChangeState(int index)
    {
        _currentState.ClientExit();
        _currentState = _states[index];
        if (!isServer)
        {
            _currentState.Init(this);
        }
        _currentState.ClientEnter();
    }

    private void Update()
    {
        if (isServer)
        {
            _currentState.Run();
        }


        if (isClient)
        {
            _currentState.ClientRun();
        }


    }

    private void FixedUpdate()
    {
        if (isServer)
            _currentState.PhysicsRun();
    }
}
