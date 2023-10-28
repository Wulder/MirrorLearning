using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DummyAttackState : NPCState
{

    [SerializeField] private float _stopAttackDistance, _comingSpeed;
    [SerializeField] private NavMeshAgent _nmAgent;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider _damageCollider;
    private float _startNavMeshAgentSpeed;
    private GameObject _player;
    public override void Enter()
    {
        base.Enter();
        _animator.SetBool("Attack", true);
        _startNavMeshAgentSpeed = _nmAgent.speed;
        _nmAgent.speed = _comingSpeed;
        
    }

    public override void PhysicsRun()
    {
        base.PhysicsRun();
        if (!_stateMachine.Npc.GetNearPlayer(_stopAttackDistance, out _player))
        {
            _stateMachine.ChangeState(_parentState);
            return;
        }
       
        _nmAgent.destination = _player.transform.position;
    }

    public override void Exit()
    {
        base.Exit();
        _nmAgent.speed = _startNavMeshAgentSpeed;
        _animator.SetBool("Attack", false);
    }
}
