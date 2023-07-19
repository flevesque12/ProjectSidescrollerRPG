using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce;

    [Header("Dash Info")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDirection { get; private set;}

    [Header("Fall modifier info")]
    [SerializeField] private float m_fallModifier = 2.5f;
    [SerializeField] private float m_LowJumpModifier = 2f;


    [Header("Collision Check")]
    [SerializeField] private LayerMask collisionMask; 
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform wallCheck; 
    [SerializeField] private float ceillingCheckDistance;
    [SerializeField] private Transform ceillingCheck;
    
    public int facingDirection{get; private set;} = 1;
    private bool facingRight = true;

    private bool ceillingDetected;

#region Components
    public Animator    anim { get; private set;}
    public Rigidbody2D rb { get; private set;}
#endregion

#region States
    public PlayerStateMachine stateMachine{get; private set;}

    public PlayerIdleState      idleState{get; private set;}
    public PlayerMoveState      moveState{get; private set;}
    public PlayerJumpState      jumpState{get; private set;}
    public PlayerAirState       airState{get; private set;}
    public PlayerDashState      dashState{get; private set;}
    public PlayerWallSlideState wallSlide{get; private set;}
    public PlayerWallJump       wallJump{get; private set;}
#endregion

    private void Awake() {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this,stateMachine,"Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState  = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this,stateMachine, "WallSlide");
        wallJump =  new PlayerWallJump(this, stateMachine, "Jump");
    }

    private void Start() {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine.Initialize(idleState);
    }


    private void Update() {
        stateMachine.currentState.Update(); 

        CheckDashInput();
    }

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

    public void SetVelocity(float _xVelocity, float _yVelocity){
        	rb.velocity = new Vector2(_xVelocity, _yVelocity);
            FlipController(_xVelocity);
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, collisionMask);

    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, collisionMask);

    public void CollisionCheck(){
        ceillingDetected = Physics2D.Raycast(ceillingCheck.position,Vector2.up,ceillingCheckDistance, collisionMask);
    }

    public void Flip(){
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0,180f,0);
    }

    public void FlipController(float _x){
        if(_x > 0 && !facingRight)
            Flip();
        else if(_x < 0 && facingRight)
            Flip();
    }

    public void FallModifierGravity()
    {
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (m_fallModifier - 1) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && !Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (m_LowJumpModifier - 1) * Time.deltaTime;
        }
    }

    protected virtual void OnDrawGizmos() {        

        if(ceillingDetected)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(ceillingCheck.position, new Vector3(ceillingCheck.position.x, ceillingCheck.position.y - ceillingCheckDistance));  
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(ceillingCheck.position, new Vector3(ceillingCheck.position.x, ceillingCheck.position.y - ceillingCheckDistance));          
        }

        if(IsGroundDetected())
        {  
            Gizmos.color = Color.green;
            Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));          
        }

        if(IsWallDetected())
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));    
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));          
        }
    }
}
