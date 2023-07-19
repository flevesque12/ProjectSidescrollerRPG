using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.velocity = new Vector2(0f, 0f);
    } 

    public override void Update()
    {
        base.Update();
        if(xInput == player.facingDirection && player.IsWallDetected()){
            return;
        }

        if(xInput != 0)
            stateMachine.ChangeState(player.moveState);
    }
    
    public override void Exit()
    {
        base.Exit();
    }
}
