using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DummyFollowState : NPCState
{
    [SerializeField] private float _stopFollowingDistance;
    [SerializeField] NavMeshAgent _nmAgent;
    [SerializeField] Animator _animator;

    private GameObject _player;

    public override void Enter()
    {
        base.Enter();
        _animator.SetBool("Walk", true);
        _nmAgent.isStopped = false;
    }

    public override void PhysicsRun()
    {
        base.PhysicsRun();
        
        if (!_stateMachine.Npc.GetNearPlayer(_stopFollowingDistance, out _player))
        {
            _stateMachine.ChangeState(_parentState);
        }
        else
        {
            _nmAgent.destination = _player.transform.position;
        }
    }

    public override void Exit()
    {
        base.Exit();
        _animator.SetBool("Walk", false);
        _nmAgent.isStopped = true;
    }
}
