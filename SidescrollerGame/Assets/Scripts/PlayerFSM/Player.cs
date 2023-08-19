using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    //make the player some hop in each attack combo to feel more alive
    [Header("Attack Info")]
    public Vector2[] attackMovement;
    
    public bool isBusy { get; private set;}

    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce;

    [Header("Dash Info")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDirection { get; private set;}

#region States
    public PlayerStateMachine stateMachine{get; private set;}

    public PlayerIdleState      idleState{get; private set;}
    public PlayerMoveState      moveState{get; private set;}
    public PlayerJumpState      jumpState{get; private set;}
    public PlayerAirState       airState{get; private set;}
    public PlayerDashState      dashState{get; private set;}
    public PlayerWallSlideState wallSlide{get; private set;}
    public PlayerWallJump       wallJump{get; private set;}

    public PlayerPrimaryAttackState primaryAttack{get; private set;}
#endregion

    protected override void Awake() {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this,stateMachine,"Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState  = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this,stateMachine, "WallSlide");
        wallJump  = new PlayerWallJump(this, stateMachine, "Jump");

        primaryAttack = new PlayerPrimaryAttackState(this,stateMachine,"Attack");
    }

    protected override void Start() {
        base.Start();

        stateMachine.Initialize(idleState);
    }


    protected override void Update() {
        base.Update();
        stateMachine.currentState.Update(); 

        CheckDashInput();        
    }

    public IEnumerator WaitTimer(float _seconds){
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void CheckDashInput(){
        if(IsWallDetected()){
            return;
        }

        dashUsageTimer -=Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0){
            dashUsageTimer = dashCooldown;
            dashDirection = Input.GetAxisRaw("Horizontal");

            if(dashDirection == 0)
                dashDirection = facingDirection;

            
            stateMachine.ChangeState(dashState);
        }
    }

    

}
