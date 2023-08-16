using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{   

    
    [Header("Movement info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;

    [Header("Attack info")]
    [SerializeField] protected LayerMask playerLayerMask;
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;

    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake() {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Start(){
        base.Start();
    }

    protected override void Update() {
        base.Update();
        stateMachine.currentState.Update();        
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, attackDistance, playerLayerMask); 

    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();

        if(IsPlayerDetected())
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDirection, transform.position.y));    
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDirection, transform.position.y));          
        }
    }

}
