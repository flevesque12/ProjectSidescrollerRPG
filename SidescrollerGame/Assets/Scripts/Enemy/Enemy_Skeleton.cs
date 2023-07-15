using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Entity
{
    private bool isAttacking;

    [Header("Move Info")]
    [SerializeField] private float moveSpeed;

    [Header("Player detection")]
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private LayerMask playerLayerMask;

    private RaycastHit2D isPlayerIsDetected;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if(isPlayerIsDetected){
            if(isPlayerIsDetected.distance > 1){
                rb.velocity = new Vector2(moveSpeed * 3.5f * faceDirection, rb.velocity.y);

                Debug.Log("<color=red>I see the player</color>");

                isAttacking = false;
            }
            else{
                Debug.Log("ATTACK " + isPlayerIsDetected.collider.gameObject.name);
                isAttacking = true;
            }
        }

        if(!isGrounded || isWallDetected)
            Flip();

        Move();
    }

    private void Move(){
        if(!isAttacking)
            rb.velocity = new Vector2(moveSpeed * faceDirection, rb.velocity.y);
    }

    protected override void CollisionCheck()
    {
        base.CollisionCheck();

        isPlayerIsDetected = Physics2D.Raycast(transform.position, Vector2.right, playerCheckDistance * faceDirection, playerLayerMask);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + playerCheckDistance * faceDirection, transform.position.y));
    }
}
