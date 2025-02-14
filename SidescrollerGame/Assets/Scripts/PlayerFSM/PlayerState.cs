using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    private string animBoolName;

    protected float stateTimer;
    protected float xInput;
    protected float yInput;
    protected bool triggerCalled;

    protected Rigidbody2D rb; 

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName){
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter(){
        //Debug.Log("<color=blue>I enter " + animBoolName + "</color>");
        rb = player.rb;
        player.anim.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update(){        
        
        stateTimer -= Time.deltaTime;

        //xInput = Input.GetAxisRaw("Horizontal");
        //there is a problem with the input the player cant stop moving
        xInput = player.inputHandler.MoveInput.x;
        Debug.Log(xInput);

        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity",rb.velocity.y);
    }

    public virtual void Exit(){        
        player.anim.SetBool(animBoolName, false);    
    }

    public virtual void AnimationFinishTrigger(){
        triggerCalled = true;
    }
    
}
