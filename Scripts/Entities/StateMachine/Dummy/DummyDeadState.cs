using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyDeadState : NPCState
{

    public override void ClientEnter()
    {
        ((Dummy)_stateMachine.Npc).Kill();
    }
}
