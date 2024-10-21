using System.Collections;
using System.Collections.Generic;
using Cinemachine.Editor;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision Check")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected LayerMask collisionMask; 
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Transform wallCheck; 
    
    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;

    public virtual int facingDirection{ get; private set; } = 1;

    public bool facingRight = true;

    protected bool ceillingDetected;
    public System.Action OnFlipped;

    #region Components
        public Animator    anim { get; private set; }
        public Rigidbody2D rb { get; private set; }
        public EntityFX fx {get; private set; }
        public SpriteRenderer sr { get; private set; }
        public CharacterStats stats { get; private set; }
        public CapsuleCollider2D cd { get; private set; }
    #endregion

    protected virtual void Awake() {
    
    }

    protected virtual void Start(){
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponentInChildren<EntityFX>();
        sr = GetComponentInChildren<SpriteRenderer>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update() {
    
    }

    public virtual void SlowEntity(float _slowPercentage, float _slowDuration)
    {

    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    public virtual void DamageImpact() => StartCoroutine("HitKnockback");    
    

    protected virtual IEnumerator HitKnockback(){
        isKnocked = true;
        //Vector2.Reflect
        rb.velocity = new Vector2(knockbackDirection.x * -facingDirection,knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration); 

        isKnocked = false;
    }

    public void ZeroVelocity(){
        if(isKnocked)
            return;

        rb.velocity = new Vector2(0f, 0f);
    }

    public void SetVelocity(float _xVelocity, float _yVelocity){

        if(isKnocked)
            return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }  
       
    

#region FlipSprite
    public virtual void Flip(){
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0,180f,0);

        if(OnFlipped != null)
            OnFlipped();
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

    
    protected virtual void OnDrawGizmos() {        
        if(attackCheck == null)
            return;

        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
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


    public void MakeTransparent(bool _transparent) {
        if(_transparent)
        {
            sr.color = Color.clear;
        }
        else
        {
            sr.color = Color.white;
        }
    }

    public virtual void Die()
    {

    }
}
