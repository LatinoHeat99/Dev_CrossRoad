using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Idle : BaseState
{
    private Player player;
    public Idle(Player _sm) : base("Idle", _sm) { player = _sm; }

    public override void Enter()
    {
        base.Enter();
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if(Input.GetMouseButtonDown(0))
            stateMachine.ChangeState(player.moveState);
    }
}
