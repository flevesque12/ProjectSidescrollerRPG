using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform playerTransform;
    private Enemy_Skeleton enemy;
    private int moveDirection;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();  
         
        playerTransform = PlayerManager.Instance.transform;          

    } 

    public override void Update()
    {
        base.Update(); 

        if(enemy.IsPlayerDetected()){
            
            stateTimer = enemy.battleTime;

            if(enemy.IsPlayerDetected().distance < enemy.attackDistance){
                if(CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
                else
                    enemy.ZeroVelocity();
                
                //return;
            }
        }
        else
        {
            if(stateTimer < 0 || Vector2.Distance(playerTransform.transform.position, enemy.transform.position) > 7)
                stateMachine.ChangeState(enemy.idleState);
        }

        if(playerTransform.position.x > enemy.transform.position.x){
            moveDirection = 1;
        }
        else if(playerTransform.position.x < enemy.transform.position.x){
            moveDirection = -1;
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDirection, rb.velocity.y);
    }
    
    public override void Exit()
    {
        base.Exit();        
    }

    private bool CanAttack(){
        if(Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown){
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        
        return false;
    }
}
