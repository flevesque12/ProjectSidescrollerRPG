using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestScripts
{

    public class TestPlayerScript : Entity
    {
        [Header("Player movement info")]
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float jumpForce = 2f;
        
        [Header("Player Attack Info")]
        [SerializeField] private float comboTime = 0.3f;
        private float comboTimeWindow;
        private bool isAttacking;
        private int comboCount = 0;
        

        [Header("Player Dash info")]
        [SerializeField] private float dashDistance;
        [SerializeField] private float dashDuration;
        private float dashTimer;
        [SerializeField] private float dashCooldown;
        private float dashCooldownTimer;

        private float xInput;
        private bool isMoving;
        

        protected override void Start()
        {
            base.Start();
        }
        

        protected override void Update()
        {
            base.Update();
            
            Movement();
            CheckInput();
        
            dashTimer -= Time.deltaTime;
            dashCooldownTimer -= Time.deltaTime;
            comboTimeWindow -= Time.deltaTime;
            
            FlipControl();
            AnimatorController();        
        }

        public void AttackOver(){
            isAttacking = false;      

            comboCount++;  

            if(comboCount > 2){
                comboCount = 0;
            }       
        }

        

        private void FlipControl(){
            if(rb.velocity.x > 0 && !facingRight){
                Flip();
            }
            else if(rb.velocity.x < 0 && facingRight){
                Flip();
            }
        }

        

        private void CheckInput(){
            xInput = Input.GetAxisRaw("Horizontal");        

            if(Input.GetKeyDown(KeyCode.Mouse0)){
                StartAttackEvent();            
            }

            if(Input.GetKeyDown(KeyCode.Space)){
                Jump();
            }

            if(Input.GetKeyDown(KeyCode.LeftShift)){
                DashAbility();
            }
        }

        private void StartAttackEvent(){
            if(!isGrounded)
                return;

            if(comboTimeWindow < 0){
                comboCount = 0;
            }

            isAttacking = true;
            comboTimeWindow = comboTime;
        }

        private void DashAbility(){
            if(dashCooldownTimer < 0 && !isAttacking){
                dashCooldownTimer = dashCooldown;
                dashTimer = dashDuration;
            }
        }

        private void Movement(){

            if(isAttacking){
                rb.velocity = new Vector2(0f, 0f);
            }
            else if(dashTimer > 0 && isGrounded){
                rb.velocity = new Vector2(faceDirection * dashDistance, 0f);
            }
            else
            {
                rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);    
            }

        }

        private void Jump(){
            if(isGrounded){
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);            
            }
        }

        private void AnimatorController(){
            isMoving = rb.velocity.x != 0;

            anim.SetFloat("yVelocity", rb.velocity.y);
            anim.SetBool("isMoving", isMoving);
            anim.SetBool("isGrounded",isGrounded);
            anim.SetBool("isDashing",dashTimer > 0);
            anim.SetBool("isAttacking",isAttacking);
            anim.SetInteger("combosCounter",comboCount);
        }
    }

}
