using UnityEngine;
public abstract class NPCState : MonoBehaviour
{
    private protected NPCStateMachine _stateMachine;
    [SerializeField] private protected NPCState _parentState;
    public virtual void Init(NPCStateMachine sm)
    {
        _stateMachine = sm;
    }
    public virtual void Enter()
    {
        if( _parentState != null )
            _parentState.Enter();
    }
    public virtual void Run() 
    {
        if (_parentState != null)
            _parentState.Run();
    }
    public virtual void PhysicsRun()
    {
        if (_parentState != null)
            _parentState.PhysicsRun();
    }
    public virtual void Exit()
    {
        {
            if (_parentState != null)
                _parentState.Exit();
        }
    }

    public virtual void ClientEnter()
    {
        {
            if (_parentState != null)
                _parentState.ClientEnter();
        }
    }
    public virtual void ClientRun()
    {
        {
            if (_parentState != null)
                _parentState.ClientRun();
        }
    }
    public virtual void ClientExit()
    {
        {
            if (_parentState != null)
                _parentState.ClientExit();
        }
    }
}
