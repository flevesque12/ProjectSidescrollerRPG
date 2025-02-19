using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    //make the player some hop in each attack combo to feel more alive
    [Header("Attack Info")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f;
    
    public bool isBusy { get; private set;}

    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    public float defaultDashSpeed;
    public float dashDirection { get; private set;}

    public SkillManager skill { get; private set; }
    public GameObject sword; //{ get; private set; }

    public PlayerInputHandler inputHandler { get; private set; }

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
    public PlayerCouterAttackState couterAttack { get; private set;}

    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set;}
    public PlayerBlackholeState blackholeState { get; private set;}
    public PlayerDeadState deadState { get; private set;}
    #endregion

    protected override void Awake() {
        base.Awake();

        inputHandler = PlayerInputHandler.Instance;

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this,stateMachine,"Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState  = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this,stateMachine, "WallSlide");
        wallJump  = new PlayerWallJump(this, stateMachine, "Jump");

        primaryAttack = new PlayerPrimaryAttackState(this,stateMachine,"Attack");
        couterAttack = new PlayerCouterAttackState(this, stateMachine, "CounterAttack");

        aimSword = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackholeState = new PlayerBlackholeState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Die");
    }

    protected override void Start() {
        base.Start();
        skill = SkillManager.Instance;
        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }


    protected override void Update() {
        base.Update();
        stateMachine.currentState.Update(); 

        CheckDashInput();        
    }

    public override void SlowEntity(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
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

        if((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.LeftShift)) && SkillManager.Instance.dash.CanUseSkill()){
            dashDirection = Input.GetAxisRaw("Horizontal");

            if(dashDirection == 0)
                dashDirection = facingDirection;

            
            stateMachine.ChangeState(dashState);
        }
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

}
