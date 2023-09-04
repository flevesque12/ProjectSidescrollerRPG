using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Mouse1))
            stateMachine.ChangeState(player.aimSword);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ChangeState(player.couterAttack);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttack);

        if(!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        if(Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected()){
            stateMachine.ChangeState(player.jumpState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }   
    
}
