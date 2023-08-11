using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestScripts
{

    public class Entity : MonoBehaviour
    {
        protected VisualDebug visualDebug;
        protected Rigidbody2D rb;
        protected Animator anim;
        [Header("Collision info")]
        [SerializeField] protected float groundCheckDistance;
        [SerializeField] protected LayerMask groundMask;
        [SerializeField] protected Transform groundCheck;
        [Space]
        [SerializeField] protected Transform wallCheck;
        [SerializeField] protected float wallCheckDistance;


        protected bool isGrounded;
        protected bool isWallDetected;

        protected int faceDirection = 1;
        protected bool facingRight = true;

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponentInChildren<Animator>();

            if(wallCheck == null)
                wallCheck = transform;
        }

        protected virtual void Update()
        {
            CollisionCheck();
        }

        protected virtual void CollisionCheck(){
            isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundMask);
            isWallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance * faceDirection, groundMask);
        }


        protected virtual void Flip(){
            faceDirection = faceDirection * -1;
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }

        protected virtual void OnDrawGizmos() {
            Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * faceDirection, wallCheck.position.y));    
        }
    }
}
