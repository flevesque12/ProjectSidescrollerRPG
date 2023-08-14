using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision Check")]
    [SerializeField] protected LayerMask collisionMask; 
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Transform wallCheck; 
    

    public virtual int facingDirection{get; private set;} = 1;

    public bool facingRight = true;

    protected bool ceillingDetected;

    //[Header("Fall modifier info")]
    private float m_fallModifier = 2.5f;
    private float m_LowJumpModifier = 2f;

    #region Components
        public Animator    anim { get; private set;}
        public Rigidbody2D rb { get; private set;}
    #endregion

    protected virtual void Awake() {
    
    }

    protected virtual void Start(){
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update() {
    
    }

    public void ZeroVelocity() => rb.velocity = new Vector2(0f, 0f);

    public void SetVelocity(float _xVelocity, float _yVelocity){
        	rb.velocity = new Vector2(_xVelocity, _yVelocity);
            FlipController(_xVelocity);
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

#region FlipSprite
    public virtual void Flip(){
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0,180f,0);
    }

    public virtual void FlipController(float _x){
        if(_x > 0 && !facingRight)
            Flip();
        else if(_x < 0 && facingRight)
            Flip();
    }
#endregion

#region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, collisionMask);

    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, collisionMask);

    
    public virtual void OnDrawGizmos() {        
       
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

#endregion

}
