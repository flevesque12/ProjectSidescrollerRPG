using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
    public PlayerPrimaryAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    

    public override void Update()
    {
        base.Update();
        //from the player state
        if(triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
   
    public override void Exit()
    {
        base.Exit();
    }   
    
}
