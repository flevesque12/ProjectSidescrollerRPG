using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{

    private int comboCounter;

    private float lastTimeAttack;
    private float comboWindow = 2;

    public PlayerPrimaryAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(comboCounter > 2 || Time.time >= lastTimeAttack + comboWindow){
            comboCounter = 0;
        }

        player.anim.SetInteger("ComboCounter", comboCounter);
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
        comboCounter++;
        lastTimeAttack = Time.time;
        
    }   
    
}
