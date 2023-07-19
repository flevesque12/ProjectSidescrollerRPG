using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJump : PlayerState
{
    public PlayerWallJump(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 0.4f;
        player.SetVelocity(5f * -player.facingDirection, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
            stateMachine.ChangeState(player.airState);

        if(player.IsGroundDetected()){
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }   
    
}
