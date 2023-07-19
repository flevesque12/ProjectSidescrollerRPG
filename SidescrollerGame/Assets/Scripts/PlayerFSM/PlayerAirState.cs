using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
   
    public override void Update()
    {
        base.Update();
        //disable for the begin of the project
        //player.FallModifierGravity();

        if(player.IsWallDetected() && xInput == player.facingDirection)
            stateMachine.ChangeState(player.wallSlide);

        if(player.IsGroundDetected()){
           stateMachine.ChangeState(player.idleState); 
        }


        if(xInput != 0){
            player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.velocity.y);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }    
}
