using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDummyState : NPCState
{
    [SerializeField] private Health _health;

    [Header("Transitions")]
    [SerializeField] private NPCState _deadState;
    [SerializeField] private NPCState _followState;
    [SerializeField] private NPCState _attackState;

    [Header("Settings")]
    [SerializeField] private float _followingTriggerDistance;
    [SerializeField] private float _attackTriggerDistance;


    private GameObject _focusedPlayer;

    public override void Enter()
    {
        _health.OnDead += OnDead;
    }

    public override void PhysicsRun()
    {
        //following detect behaviour
        if (_stateMachine.Npc.GetNearPlayerInRange(_attackTriggerDistance, _followingTriggerDistance, out _focusedPlayer))
        {

            if (_stateMachine.CurrentState != _followState)
            {
                ((Dummy)_stateMachine.Npc).SetFocusPlayer(_focusedPlayer);

                _stateMachine.ChangeState(_followState);
                return;
            }
        }

        //attack detect behaviour
        if (_stateMachine.Npc.GetNearPlayer(_attackTriggerDistance, out _focusedPlayer))
        {
            if (_stateMachine.CurrentState != _attackState)
            {
                ((Dummy)_stateMachine.Npc).SetFocusPlayer(_focusedPlayer);
                _stateMachine.ChangeState(_attackState);

                return;
            }
        }
    }

    public override void Exit()
    {
        _health.OnDead -= OnDead;
    }

    void OnDead()
    {
        _stateMachine.ChangeState(_deadState);
    }
}
