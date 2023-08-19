using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{

    private int comboCounter;

    private float lastTimeAttack;
    private float comboWindow = 2;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0; //this fix bug

        if(comboCounter > 2 || Time.time >= lastTimeAttack + comboWindow){
            comboCounter = 0;
        }

        player.anim.SetInteger("ComboCounter", comboCounter);
        //can be set here to make the player attack faster
        //player.anim.speed = 1.2f;

        //ChooseAttackDirection
        float attackDirection = player.facingDirection;
        if(xInput != 0){
            attackDirection = xInput;
        }
        
        //make the player advance a number of step between attack combo
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDirection, player.attackMovement[comboCounter].y);

        stateTimer = 0.1f;
    }
    

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0){
            player.ZeroVelocity();
        }

        //from the player state
        if(triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
   
    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("WaitTimer", 0.15f);

        comboCounter++;
        lastTimeAttack = Time.time;        
    }   
    
}
