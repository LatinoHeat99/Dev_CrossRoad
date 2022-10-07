using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Move : BaseState
{
    private Player player;
    private Vector3 movePos;
    
    public Move(Player _sm) : base("Move", _sm) { player = _sm; }

    public override void Enter()
    {
        base.Enter();

        movePos = player.transform.position + player.transform.forward * 5.0f;
        player.StartCoroutine(Moving());
    }
    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    IEnumerator Moving()
    {
        while (!(Vector3.Distance(player.transform.position, movePos) < Mathf.Epsilon))
        {
            //transform.position = Vector3.SmoothDamp(transform.position, movePos, ref velocity, 0.5f);
            player.transform.position = Vector3.MoveTowards(player.transform.position, movePos, Time.deltaTime * player.GetSpeed());
            //Debug.Log("Move " + player.transform.position);
            yield return null;
        }
        player.addThroughCount();
        stateMachine.ChangeState(player.idleState);
    }
}
