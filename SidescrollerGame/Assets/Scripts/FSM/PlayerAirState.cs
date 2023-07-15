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

        if(rb.velocity.y == 0){
           stateMachine.ChangeState(player.idleState); 
        }
    }

    public override void Exit()
    {
        base.Exit();
    }    
}
